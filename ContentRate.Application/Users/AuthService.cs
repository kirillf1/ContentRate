using Ardalis.Result;
using ContentRate.Application.Contracts.Users;
using ContentRate.Domain.Users;

namespace ContentRate.Application.Users
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        public AuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<Result<bool>> HasUser(string name)
        {
            try
            {
                return await userRepository.HasUser(name);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<UserTitle>> Login(LoginModel loginModel)
        {
            try
            {
                var user = await userRepository.TryGetUser(loginModel.Name, loginModel.Password);
                if (user is null)
                    return Result.NotFound();
                return Result.Success(new UserTitle { Id = user.Id, Name = user.Name });

            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<UserTitle>> Register(RegisterModel registerModel)
        {
            try
            {
                var hasUser = await userRepository.HasUser(registerModel.Name);
                if (hasUser)
                    return Result.Error("User with this name exists!");
                var newUser = new User(Guid.NewGuid(), registerModel.Name, registerModel.Password);
                await userRepository.AddUser(newUser);
                await userRepository.SaveChanges();
                return Result.Success(new UserTitle { Id = newUser.Id, Name = newUser.Name });

            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
