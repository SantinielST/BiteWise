using BiteWise.BLL.Models;

namespace BiteWise.Contracts.UserDtos;

public class UserDto
{
    public string Id => User?.Id ?? string.Empty;
    public string? Email { get; set; }
    public User? User { get; set; }
    public string? Image { get; set; }
    public string? Status { get; set; }
    public string? About { get; set; }
    public IList<string>? Roles { get; set; }
    public IList<Article>? Articles { get; set; }

    public UserDto() { }

    public UserDto(User user)
    {
        User = user;
    }
}