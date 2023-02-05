using Ardalis.Result;
using ContentRate.Application.Contracts.Users;
using ContentRate.Domain.Users;

namespace ContentRate.Application.Users
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        // рефакторниг
        public async Task<Result<IEnumerable<UserTitle>>> GetAllUsers()
        {
            try
            {
                var users = await userRepository.GetUsers(new UserSearchCreteria(IncludeMockUsers:true));
                return Result.Success(users.Select(c => new UserTitle { Id = c.Id, Name = c.Name }));
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<UserTitle>>> GetNotMockUsers()
        {
            try
            {
                var users = await userRepository.GetUsers(new UserSearchCreteria(IncludeMockUsers: false));
                return Result.Success(users.Select(c => new UserTitle { Id = c.Id, Name = c.Name }));
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
