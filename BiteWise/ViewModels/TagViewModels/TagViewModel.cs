using System.ComponentModel.DataAnnotations;

namespace BiteWise.ViewModels.TagViewModels;

public class TagViewModel
{
    [Required(ErrorMessage = "Название тега обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Название")]
    public string? Name { get; set; }
}
