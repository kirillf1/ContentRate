using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.Protos;
using Grpc.Core;

namespace ContentRate.GrpcService.GrpcServices.Users
{
    public class AuthServiceGrpc : Protos.AuthService.AuthServiceBase
    {
        private readonly IAuthService authService;

        public AuthServiceGrpc(IAuthService authService)
        {
            this.authService = authService;
        }
        public override async Task<IsUniqueUser> HasUser(UserCheckGrpc request, ServerCallContext context)
        {
            var result = await authService.HasUser(request.Name);
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return new IsUniqueUser { Result = !result.Value };
        }
        public override async Task<UserTitleGrpc> Login(LoginMessageGrpc request, ServerCallContext context)
        {
            var result = await authService.Login(new Application.Contracts.Users.LoginModel
            {
                Name = request.Name,
                Password = request.Password,
            });
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return UserConverter.ConvertToUserTitleGrpc(result.Value);
        }
        public override async Task<UserTitleGrpc> Register(RegisterMessageGrpc request, ServerCallContext context)
        {
            var result = await authService.Register(new Application.Contracts.Users.RegisterModel
            {
                Name = request.Name,
                Password = request.Password,
            });
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return UserConverter.ConvertToUserTitleGrpc(result.Value);
        }
    }
}
