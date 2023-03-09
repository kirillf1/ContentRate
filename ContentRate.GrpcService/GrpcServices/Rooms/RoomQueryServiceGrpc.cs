using ContentRate.Application.Rooms;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ContentRate.GrpcService.GrpcServices.Rooms
{
    public class RoomQueryServiceGrpc : Protos.RoomQueryService.RoomQueryServiceBase
    {
        private readonly IRoomQueryService queryService;

        public RoomQueryServiceGrpc(IRoomQueryService queryService)
        {
            this.queryService = queryService;
        }
        public override async Task GetPersonalRoomTitles(UserIdGrpc request, IServerStreamWriter<RoomTitleGrpc> responseStream, ServerCallContext context)
        {
            var result = await queryService.GetPersonalRoomTitles(Guid.Parse(request.Id));
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            foreach (var roomTitle in result.Value)
            {
                RoomTitleGrpc roomTitleGrpc = RoomConverter.ConvertRoomTitleToGrpc(roomTitle);
                await responseStream.WriteAsync(roomTitleGrpc);
            }
        }

        public override async Task GetRoomTitles(Empty request, IServerStreamWriter<RoomTitleGrpc> responseStream, ServerCallContext context)
        {
            var result = await queryService.GetRoomTitles();
            if (result.Status == Ardalis.Result.ResultStatus.Error)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            if (result.Status == Ardalis.Result.ResultStatus.NotFound)
                return;
            foreach (var roomTitle in result.Value)
            {
                RoomTitleGrpc roomTitleGrpc = RoomConverter.ConvertRoomTitleToGrpc(roomTitle);
                await responseStream.WriteAsync(roomTitleGrpc);
            }
        }
        
    }
}
