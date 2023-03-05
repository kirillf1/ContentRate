using Ardalis.Result;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;
using Grpc.Core;
using System.Reflection.PortableExecutable;
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
                Console.WriteLine("check");
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
                var call = client.RegisterAsync(new Protos.RegisterMessageGrpc
                {
                    Name = registerModel.Name,
                    Password = registerModel.Password,
                });
                Metadata headers = await call.ResponseHeadersAsync;
                var authToken = headers.GetValue("Authorization");
                var userTitleGrpc = await call.ResponseAsync;
                await tokenProvider.RefreshToken(authToken!);
                return UserConverter.ConvertGrpcToUserTitle(userTitleGrpc);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
