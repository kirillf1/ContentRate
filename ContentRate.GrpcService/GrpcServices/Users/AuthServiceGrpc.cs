using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.GrpcService.Authorization;
using ContentRate.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace ContentRate.GrpcService.GrpcServices.Users
{
    public class AuthServiceGrpc : Protos.AuthService.AuthServiceBase
    {
        private readonly IAuthService authService;
        private readonly ITokenGenerator tokenGenerator;

        public AuthServiceGrpc(IAuthService authService, ITokenGenerator tokenGenerator)
        {
            this.authService = authService;
            this.tokenGenerator = tokenGenerator;
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
            var token = await tokenGenerator.GenerateToken(result.Value);
            await context.WriteResponseHeadersAsync(new Metadata() { { "Authorization", token } });
            return UserConverter.ConvertToUserTitleGrpc(result.Value);
        }
    }
}
