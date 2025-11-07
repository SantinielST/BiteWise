using BiteWise.BLL.Models;

namespace BiteWise.ViewModels;

public class SearchViewModel
{
    public List<User>? UserList { get; set; }
    public string? RoleName { get; set; }
    public string? UserId { get; set; }
}