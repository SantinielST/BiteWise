using System.ComponentModel.DataAnnotations;

namespace BiteWise.Contracts.TagDtos;

public class TagDto
{
    [Required(ErrorMessage = "Название тега обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Название")]
    public string? Name { get; set; }
}
