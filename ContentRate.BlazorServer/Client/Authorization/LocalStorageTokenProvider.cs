using Blazored.LocalStorage;
using ContentRate.Application.Users;

namespace ContentRate.BlazorServer.Client.Authorization
{
    public class LocalStorageTokenProvider : ITokenProvider
    {
        public LocalStorageTokenProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        private readonly ILocalStorageService localStorageService;

        public async Task<string> GetToken()
        {
            var token = await localStorageService.GetItemAsync<string>("AccessToken");
            return token ?? string.Empty;
        }

        public async Task RefreshToken(string token)
        {
            await localStorageService.SetItemAsStringAsync("AccessToken", token);
        }
    }
}
