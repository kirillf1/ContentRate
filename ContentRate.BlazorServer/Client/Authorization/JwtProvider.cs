using ContentRate.Application.Users;

namespace ContentRate.BlazorServer.Client.Authorization
{
    public class JwtProvider : ITokenProvider
    {
        string token = "";
        public Task<string> GetToken()
        {
            return Task.FromResult(token);
        }

        public Task RefreshToken(string token)
        {
            this.token = token;
            return Task.CompletedTask;
        }
    }
}
