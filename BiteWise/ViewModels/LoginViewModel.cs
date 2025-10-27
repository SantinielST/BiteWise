using System.ComponentModel.DataAnnotations;

namespace BiteWise.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email обязательно для заполнения")]
    [EmailAddress]
    [Display(Name = "Email", Prompt = "Введите email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Обязательно пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль", Prompt = "Введите пароль")]
    public string? Password { get; set; }

    [Display(Name = "Запомнить?")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}