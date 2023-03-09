using ContentRate.Application.Users;
using ContentRate.ViewModels.Users;
using FluentValidation;

namespace ContentRate.ViewModels.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        private readonly IAuthService authService;

        public RegisterValidator(IAuthService authService)
        {
            this.authService = authService;
            RuleFor(c=>c.Name).NotEmpty()
                .WithMessage("Имя не должно быть пустым")
                .MustAsync(async (name, cancellation) =>
                {
                    var hasUserResult = await authService.HasUser(name);
                    return hasUserResult.IsSuccess && hasUserResult.Value;
                })
                .WithMessage("Пользователь с таким именем уже есть!");
            RuleFor(c => c.Password).MinimumLength(5)
                .WithMessage("Пароль должен быть больше 5 знаков");
            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password)
                .WithMessage("Пароли должны совпадать");           
        }
    }
}
