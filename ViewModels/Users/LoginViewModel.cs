using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ReactiveUI;
using System.Reactive;

namespace ContentRate.ViewModels.Users
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService authService;
        private LoginModel loginModel;
        public LoginViewModel(IAuthService authService)
        {
            this.authService = authService;
            loginModel = new LoginModel() { Name = "", Password = "" };
            LoginCommand = ReactiveCommand.CreateFromTask(TryLogin);
        }
        public ReactiveCommand<Unit, UserTitle?> LoginCommand { get; }
        public string Name 
        { 
            get => loginModel.Name; 
            set
            {
                loginModel.Name = value;
                this.RaisePropertyChanged();
            }
        }
        public string Password
        {
            get => loginModel.Password;
            set
            {
                loginModel.Password = value;
                this.RaisePropertyChanged();
            }
        }
        private async Task<UserTitle?> TryLogin()
        {
            var loginResult = await authService.Login(loginModel);
            if (!loginResult.IsSuccess)
                return null;
            return loginResult.Value;
        }
    }
}
