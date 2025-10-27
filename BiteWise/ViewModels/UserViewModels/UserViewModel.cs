using BiteWise.BLL.Models;

namespace BiteWise.ViewModels.UserViewModels;

public class UserViewModel
{
    public string Id => User?.Id ?? string.Empty;
    public string? Email { get; set; }
    public User? User { get; set; }
    public string? Image { get; set; }
    public string? Status { get; set; }
    public string? About { get; set; }
    public IList<string>? Roles { get; set; }
    public IList<Article>? Articles { get; set; }

    public UserViewModel() { }

    public UserViewModel(User user)
    {
        User = user;
    }
}