using Microsoft.AspNetCore.Identity;

namespace BiteWise.DLL.Entities;

public class UserEntity : IdentityUser
{
    public string? Login { get; set; }
    public required string Password { get; set; }
}
