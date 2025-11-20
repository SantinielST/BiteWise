using BiteWise.BLL.Models;

namespace BiteWise.Contracts;

public class SearchViewDto
{
    public List<User>? UserList { get; set; }
    public string? RoleName { get; set; }
    public string? UserId { get; set; }
}