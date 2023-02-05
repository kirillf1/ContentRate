using ContentRate.Application.Contracts.Users;
using ContentRate.Protos;

namespace ContentRate.GrpcExtensions.Helpers
{
    public static class UserConverter
    {
        public static UserTitleGrpc ConvertToUserTitleGrpc(UserTitle creator)
        {
            return new UserTitleGrpc
            {
                Id = creator.Id.ToString(),
                Name = creator.Name
            };
        }
        public static UserTitle ConvertGrpcToUserTitle(UserTitleGrpc userTitleGrpc)
        {
            return new UserTitle
            {
                Id = Guid.Parse(userTitleGrpc.Id),
                Name = userTitleGrpc.Name,
            };
        }
    }
}
