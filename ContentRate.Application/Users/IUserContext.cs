using ContentRate.Application.Contracts.Users;

namespace ContentRate.Application.Users
{
    public interface IUserContext
    {
        Task<UserTitle?> TryGetCurrentUser();
    }
}
