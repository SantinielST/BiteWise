using BiteWise.ViewModels.UserViewModels;

namespace BiteWise.ViewModels;

public class MainViewModel
{
    public RegisterViewModel RegisterView { get; set; }

    public LoginViewModel LoginView { get; set; }

    public DashBoardViewModel DashBoardView { get; set; }

    public MainViewModel()
    {
        RegisterView = new RegisterViewModel();
        LoginView = new LoginViewModel();
        DashBoardView = new DashBoardViewModel();
    }
}