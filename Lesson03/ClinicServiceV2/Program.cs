using ClinicService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ClinicServiceV2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configure Kestrel

        //builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.Listen(IPAddress.Any, 5001, listenOptions =>
        //    {
        //        listenOptions.Protocols = HttpProtocols.Http2;
        //    });
        //    options.Listen(IPAddress.Any, 5101, listenOptions =>
        //    {
        //        listenOptions.Protocols = HttpProtocols.Http1;
        //    });
        //});

        #endregion

        #region Configure Grpc

        builder.Services
            .AddGrpc()
            .AddJsonTranscoding();


        #endregion

        #region Configure DB

        builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        }, ServiceLifetime.Scoped);

        #endregion

        #region Configure Identity

        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 5;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ClinicServiceDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddAuthentication();

        #endregion

        #region Configure JWT

        var secret = builder.Configuration.GetSection("JwtSettings")["Secret"];
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateAudience = false,
                ValidateIssuer = false,
            };
        });

        #endregion

        #region Configure Swagger

        // https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-openapi?view=aspnetcore-7.0
        builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "Clinic Service", Version = "v1" });

            var filePath = Path.Combine(System.AppContext.BaseDirectory, "ClinicServiceV2.xml");
            c.IncludeXmlComments(filePath);
            c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Scheme = "Bearer",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });

        #endregion


        // Add services to the container.
        builder.Services.AddAuthorization();

        #region Automapper

        builder.Services.AddAutoMapper(typeof(Program));

        #endregion

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        // Configure the HTTP request pipeline.
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled= true });

        app.MapGrpcService<ClinicServiceV2.Services.ClinicService>()
            .EnableGrpcWeb();
        app.MapGrpcService<ClinicServiceV2.Services.AuthService>()
            .EnableGrpcWeb();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client");

        app.Run();
    }
}
