using System.ComponentModel.DataAnnotations;

namespace BiteWise.Contracts.TagDtos;

public class EditTagDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Название тега обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Название")]
    public string? Name { get; set; }
}