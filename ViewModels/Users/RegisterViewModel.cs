using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ContentRate.ViewModels.Validators;
using FluentValidation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace ContentRate.ViewModels.Users
{
    public class RegisterViewModel : ViewModelBase
    {
        public RegisterViewModel(IAuthService authService)
        {
            this.authService = authService;
            registerModel = new RegisterModel()
            {
                Name = "",
                Password = "",
            };
            ConfirmPassword = "";
            Validator = new RegisterValidator(authService);
            RegisterCommand = ReactiveCommand.CreateFromTask(TryRegister);
        }
        private readonly IAuthService authService;
        private RegisterModel registerModel;
        public ReactiveCommand<Unit, UserTitle?> RegisterCommand { get; }
        public AbstractValidator<RegisterViewModel> Validator { get; }
        public string Password
        {
            get => registerModel.Password;
            set
            {

                registerModel.Password = value;
                this.RaisePropertyChanged();
            }
        }
        [Reactive]
        public string ConfirmPassword { get; set; }
        public string Name
        {
            get => registerModel.Name;
            set
            {
                registerModel.Name = value;
                this.RaisePropertyChanged();
            }
        }
        private async Task<UserTitle?> TryRegister()
        {
            var validateResult = await Validator.ValidateAsync(this);
            if (!validateResult.IsValid)
                return null;
            var registerResult = await authService.Register(registerModel);
            if (registerResult.IsSuccess)
                return registerResult.Value;
            return null;

        }
    }
}
