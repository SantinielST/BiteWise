namespace BiteWise.BLL.Models;

public class User
{
    public string? Id { get; set; }

    public required string UserName { get; set; }

    public string? Password { get; set; }

    public required string Email { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public string? About { get; set; }

    public  IList<string>? Roles { get; set; }

    public  IList<Article>? Articles { get; set; }

    public User()
    {
        Image = "https://thispersondoesnotexist.com";
        Status = "Я новенький";
        About = "Информация обо мне.";
    }
}