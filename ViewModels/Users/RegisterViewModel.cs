using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace ContentRate.ViewModels.Users
{
    public class RegisterViewModel : ViewModelBase, IValidatableViewModel
    {
        private readonly IAuthService authService;
        private RegisterModel registerModel;
        public ValidationContext ValidationContext { get; } = new ValidationContext();
        public RegisterViewModel(IAuthService authService)
        {
            this.authService = authService;
            registerModel = new RegisterModel()
            {
                Name = "",
                Password = "",
            };
            ConfirmPassword = "";
            IObservable<bool> passwordsObservable =
                this.WhenAnyValue(
                      x => x.Password,
                      x => x.ConfirmPassword,
                      (password, confirmation) => password == confirmation);

            this.ValidationRule(vm => vm.ConfirmPassword,
                passwordsObservable,
                "Пароли должны совпадать.");

            this.ValidationRule(viewModel => viewModel.Name,
                  name => !string.IsNullOrWhiteSpace(name),
                  "Имя не должно быть пустым");
        }
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
    }
}
