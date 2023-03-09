using Ardalis.Result;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.Protos;
using Grpc.Core;
using AuthService = ContentRate.Protos.AuthService;

namespace ContentRate.GrpcClient.Users
{
    public class AuthClientGrpcService : IAuthService
    {
        private readonly AuthService.AuthServiceClient client;
        private readonly ITokenProvider tokenProvider;

        public AuthClientGrpcService(AuthService.AuthServiceClient client,ITokenProvider tokenProvider)
        {
            this.client = client;
            this.tokenProvider = tokenProvider;
        }
        public async Task<Result<bool>> HasUser(string name)
        {
            try
            {
                var isUniqueUser = await client.HasUserAsync(new Protos.UserCheckGrpc { Name = name });
                return isUniqueUser.Result;
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
                var call = client.LoginAsync(new Protos.LoginMessageGrpc
                {
                    Name = loginModel.Name,
                    Password = loginModel.Password,
                });
                var userTitleGrpc = await SetAuthorizationToken(call);
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
                var call = client.RegisterAsync(new Protos.RegisterMessageGrpc
                {
                    Name = registerModel.Name,
                    Password = registerModel.Password,
                });
                UserTitleGrpc userTitleGrpc = await SetAuthorizationToken(call);
                return UserConverter.ConvertGrpcToUserTitle(userTitleGrpc);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        private async Task<UserTitleGrpc> SetAuthorizationToken(AsyncUnaryCall<UserTitleGrpc> call)
        {
            Metadata headers = await call.ResponseHeadersAsync;
            var authToken = headers.GetValue("Authorization");
            var userTitleGrpc = await call.ResponseAsync;
            await tokenProvider.RefreshToken(authToken!);
            return userTitleGrpc;
        }
    }
}
