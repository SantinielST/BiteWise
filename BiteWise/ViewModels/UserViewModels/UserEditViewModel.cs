using BiteWise.BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace BiteWise.ViewModels.UserViewModels;

public class UserEditViewModel
{
    [Required]
    [Display(Name = "Идентификатор пользователя")]
    public required string UserId { get; set; }

    [EmailAddress]
    [Display(Name = "Email", Prompt = "example.com")]
    public required string Email { get; set; }

    [DataType(DataType.ImageUrl)]
    [Display(Name = "Фото", Prompt = "Укажите ссылку на картинку")]
    public string? Image { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Статус", Prompt = "Введите статус")]
    public string? Status { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "О себе", Prompt = "Введите данные о себе")]
    public string? About { get; set; }

    public IList<string>? Roles { get; set; }
    public IList<Article>? Articles { get; set; }

    public string? ReturnUrl { get; set; }
}