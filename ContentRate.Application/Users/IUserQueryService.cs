using Ardalis.Result;
using ContentRate.Application.Contracts.Users;

namespace ContentRate.Application.Users
{
    public interface IUserQueryService
    {
        public Task<Result<IEnumerable<UserTitle>>> GetNotMockUsers();
        public Task<Result<IEnumerable<UserTitle>>> GetAllUsers();
    }
}
