using Microsoft.AspNetCore.Identity;

namespace ClinicService.Data;
public class User : IdentityUser
{
    public string Firstname { get; set; }
    public string LastName { get; set; }
}
