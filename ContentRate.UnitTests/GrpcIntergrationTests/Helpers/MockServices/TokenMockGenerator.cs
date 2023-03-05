using ContentRate.Application.Contracts.Users;
using ContentRate.GrpcService.Authorization;

namespace ContentRate.UnitTests.GrpcIntergrationTests.Helpers.MockServices
{
    internal class TokenMockGenerator : ITokenGenerator
    {
        public Task<string> GenerateToken(UserTitle userTitle)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
