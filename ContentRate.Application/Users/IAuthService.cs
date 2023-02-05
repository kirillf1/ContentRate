using Ardalis.Result;
using ContentRate.Application.Contracts.Users;

namespace ContentRate.Application.Users
{
    public interface IAuthService
    {
        public Task<Result<UserTitle>> Login(LoginModel loginModel);
        public Task<Result<UserTitle>> Register(RegisterModel registerModel);
        public Task<Result<bool>> HasUser(string name);
    }
}
