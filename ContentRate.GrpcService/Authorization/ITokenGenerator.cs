using ContentRate.Application.Contracts.Users;

namespace ContentRate.GrpcService.Authorization
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(UserTitle userTitle);
    }
}
