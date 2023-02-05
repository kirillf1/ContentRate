using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ContentRate.GrpcService.GrpcServices.Users
{
    public class UserQueryServiceGrpc : Protos.UserQueryService.UserQueryServiceBase
    {
        private readonly IUserQueryService queryService;

        public UserQueryServiceGrpc(IUserQueryService queryService)
        {
            this.queryService = queryService;
        }
        public override async Task GetAllUsers(Empty request, IServerStreamWriter<UserTitleGrpc> responseStream, ServerCallContext context)
        {
            var result = await queryService.GetAllUsers();
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            foreach (var userTitle in result.Value)
            {
                await responseStream.WriteAsync(UserConverter.ConvertToUserTitleGrpc(userTitle));
            }
        }
        public async override Task GetNotMockUsers(Empty request, IServerStreamWriter<UserTitleGrpc> responseStream, ServerCallContext context)
        {
            var result = await queryService.GetNotMockUsers();
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            foreach (var userTitle in result.Value)
            {
                await responseStream.WriteAsync(UserConverter.ConvertToUserTitleGrpc(userTitle));
            }
        }
    }
}
