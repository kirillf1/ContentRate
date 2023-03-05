using ContentRate.Application.Users;

namespace ContentRate.UnitTests.GrpcIntergrationTests.Helpers.MockServices
{
    internal class TokenMockProvider : ITokenProvider
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
