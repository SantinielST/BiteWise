using System.ComponentModel.DataAnnotations;

namespace BiteWise.Contracts.UserDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Email обязательно для заполнения")]
    [EmailAddress]
    [Display(Name = "Email", Prompt = "example.com")]
    public string? EmailReg { get; set; }

    [Required(ErrorMessage = "Пароль обязательно для заполнения")]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[.!@#$%^&*()_+\-]).{5,}$",
        ErrorMessage = "Пароль должен быть не менее 5 символов и содержать цифру, заглавную и строчную буквы, и спецсимвол: .!@#$%^&*()_-+.")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль", Prompt = "Введите пароль")]
    [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
    public string? PasswordReg { get; set; }

    [Required(ErrorMessage = "Обязательно подтвердите пароль")]
    [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
    public string? PasswordConfirm { get; set; }

    public string? UserName => EmailReg;
}