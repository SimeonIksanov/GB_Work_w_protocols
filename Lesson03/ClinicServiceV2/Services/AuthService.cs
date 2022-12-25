using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using ClinicService.Data;
using ClinicServiceNamespace;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static ClinicServiceNamespace.AuthService;

namespace ClinicServiceV2.Services;

[Authorize]
public class AuthService : AuthServiceBase
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private User? _user;

    public AuthService(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    [AllowAnonymous]
    public override async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, ServerCallContext context)
    {
        if (!await ValidateUser(request))
            return new AuthenticateResponse { ErrCode = 1005, ErrMessage = "Unauthorized" };
        return new AuthenticateResponse { Token = CreateToken() };
    }

    [AllowAnonymous]
    public override async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, ServerCallContext context)
    {
        var user = _mapper.Map<User>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return new RegisterUserResponse { ErrCode = 1004, ErrMessage = string.Join(", ", result.Errors.Select(x => $"Code: {x.Code}; Description: {x.Description}"))};

        return new RegisterUserResponse { ErrCode = 0, ErrMessage = string.Empty };
    }

    private async Task<bool> ValidateUser(AuthenticateRequest request)
    {
        _user = await _userManager.FindByNameAsync(request.Username);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, request.Password));
        return result;
    }

    private string CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings")["Secret"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims()
    {
        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, _user.UserName) };

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken(claims: claims,
                                                expires: DateTime.Now.AddMinutes(Convert.ToInt32(jwtSettings["expires"])),
                                                signingCredentials: signingCredentials);
        return tokenOptions;
    }
}
