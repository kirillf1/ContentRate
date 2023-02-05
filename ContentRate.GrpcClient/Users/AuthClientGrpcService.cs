using Ardalis.Result;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;

namespace ContentRate.GrpcClient.Users
{
    internal class AuthClientGrpcService : IAuthService
    {
        private readonly ClientProtos.AuthService.AuthServiceClient client;

        public AuthClientGrpcService(ClientProtos.AuthService.AuthServiceClient client)
        {
            this.client = client;
        }
        public async Task<Result<bool>> HasUser(string name)
        {
            try
            {
                var isUniqueUser = await client.HasUserAsync(new Protos.UserCheckGrpc { Name = name });
                return !isUniqueUser.Result;
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
                var userTitleGrpc = await client.LoginAsync(new Protos.LoginMessageGrpc
                {
                    Name = loginModel.Name,
                    Password = loginModel.Password,
                });
                return UserConverter.ConvertGrpcToUserTitle(userTitleGrpc);
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<UserTitle>> Register(RegisterModel registerModel)
        {
            try
            {
                var userTitleGrpc = await client.RegisterAsync(new Protos.RegisterMessageGrpc
                {
                    Name = registerModel.Name,
                    Password = registerModel.Password,
                });
                return UserConverter.ConvertGrpcToUserTitle(userTitleGrpc);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
