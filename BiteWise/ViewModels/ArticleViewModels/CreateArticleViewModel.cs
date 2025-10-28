using BiteWise.BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace BiteWise.ViewModels.ArticleViewModels;

public class CreateArticleViewModel
{
    [Required(ErrorMessage = "Заголовок обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Заголовок")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Текст статьи обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Текст статьи")]
    public string? Content { get; set; }
    public string? UserEntityId { get; set; }

    [Required(ErrorMessage = "Иллюстрация для статьи обязательно для заполнения")]
    [DataType(DataType.ImageUrl)]
    [Display(Name = "Иллюстрация")]
    public string? Image { get; set; }

    public required DateTime Created { get; set; }

    public IList<Tag>? AllTags { get; set; }
    public IList<string>? SelectedTagsIds { get; set; }
}