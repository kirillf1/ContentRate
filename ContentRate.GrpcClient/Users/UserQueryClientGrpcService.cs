using Ardalis.Result;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using ContentRate.GrpcExtensions.Helpers;
using Grpc.Core;


namespace ContentRate.GrpcClient.Users
{
    public class UserQueryClientGrpcService : IUserQueryService
    {
        private readonly ClientProtos.UserQueryService.UserQueryServiceClient client;

        public UserQueryClientGrpcService(ClientProtos.UserQueryService.UserQueryServiceClient client)
        {
            this.client = client;
        }
        public async Task<Result<IEnumerable<UserTitle>>> GetAllUsers()
        {
            try
            {
                var usersGrpc = client.GetAllUsers(new Google.Protobuf.WellKnownTypes.Empty());
                var users = new List<UserTitle>();
                await foreach (var userGrpc in usersGrpc.ResponseStream.ReadAllAsync())
                {
                    users.Add(UserConverter.ConvertGrpcToUserTitle(userGrpc));
                }
                return users;
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<UserTitle>>> GetNotMockUsers()
        {
            try
            {
                var usersGrpc = client.GetNotMockUsers(new Google.Protobuf.WellKnownTypes.Empty());
                var users = new List<UserTitle>();
                await foreach (var userGrpc in usersGrpc.ResponseStream.ReadAllAsync())
                {
                    users.Add(UserConverter.ConvertGrpcToUserTitle(userGrpc));
                }
                return users;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
