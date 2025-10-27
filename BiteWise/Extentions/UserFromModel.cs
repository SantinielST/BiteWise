using BiteWise.BLL.Models;
using BiteWise.ViewModels.UserViewModels;

namespace BiteWise.Extentions;

public static class UserFromModel
{
    public static User Convert(this User user, UserEditViewModel userEditViewModel)
    {
        user.UserName = userEditViewModel.Email;
        user.About = userEditViewModel.About;
        user.Status = userEditViewModel.Status;
        user.Roles = userEditViewModel.Roles;
        user.Email = userEditViewModel.Email;
        user.Image = userEditViewModel.Image;

        return user;
    }
}