using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;

namespace ContentRate.GrpcService.Authorization
{
    public class UserContextInMemory : IUserContext
    {
        private readonly UserTitle? userTitle;
        public UserContextInMemory(UserTitle? userTitle)
        {
            this.userTitle = userTitle;
        }
        public Task<UserTitle?> TryGetCurrentUser()
        {
            return Task.FromResult(userTitle);
        }
    }
}
