using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;

namespace ContentRate.BlazorComponents.States
{
    // чуток неправильное место хранения данного класса, нужно будет изменить (например создать библиотеку абстракций
    public class UserAuthState
    {
        private readonly IUserContext userContext;
        private readonly ITokenProvider tokenProvider;
        public event Action<UserTitle?>? OnUserChanged;
        public UserAuthState(IUserContext userContext, ITokenProvider tokenProvider)
        {
            this.userContext = userContext;
            this.tokenProvider = tokenProvider;
        }
        public async Task<UserTitle?> GetCurrentUserTitle()
        {
            return await userContext.TryGetCurrentUser();
        }
        public async Task<bool> HasUserAuthenticated()
        {
            return await userContext.TryGetCurrentUser() is not null;
        }
        public async Task SignOut()
        {
            await tokenProvider.RefreshToken("");
            OnUserChanged?.Invoke(null);
        }
        public void SetNewUser(UserTitle userTitle)
        {
            OnUserChanged?.Invoke(userTitle);
        }
    }
}
