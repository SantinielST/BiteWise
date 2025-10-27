using Microsoft.AspNetCore.Identity;

namespace BiteWise.DLL.Entities;

public class UserEntity : IdentityUser
{
    public string? Image { get; set; }

    public string? Status { get; set; }

    public string? About { get; set; }
}
