using Ardalis.Result;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using ContentRate.GrpcExtensions.Helpers;
using Grpc.Core;
using ContentRate.Protos;
using RoomQueryService = ContentRate.Protos.RoomQueryService;

namespace ContentRate.GrpcClient.Rooms
{
    public class RoomQueryClientGrpcService : IRoomQueryService
    {
        private readonly RoomQueryService.RoomQueryServiceClient client;

        public RoomQueryClientGrpcService(RoomQueryService.RoomQueryServiceClient client)
        {
            this.client = client;
        }
        public async Task<Result<IEnumerable<RoomTitle>>> GetPersonalRoomTitles(Guid userId)
        {
            try
            {
                var roomTitllesGrpc = client.GetPersonalRoomTitles(new Protos.UserIdGrpc { Id = userId.ToString() });
                var roomTitles = new List<RoomTitle>();
                await foreach (var roomTitleGrpc in roomTitllesGrpc.ResponseStream.ReadAllAsync())
                {
                    roomTitles.Add(RoomConverter.ConvertGrpcToRoomTitle(roomTitleGrpc));
                }
                return roomTitles;
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<RoomTitle>>> GetRoomTitles()
        {
            try
            {
                var roomTitllesGrpc = client.GetRoomTitles(new Google.Protobuf.WellKnownTypes.Empty());
                var roomTitles = new List<RoomTitle>();
                await foreach (var roomTitleGrpc in roomTitllesGrpc.ResponseStream.ReadAllAsync())
                {
                    roomTitles.Add(RoomConverter.ConvertGrpcToRoomTitle(roomTitleGrpc));
                }
                return roomTitles;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
    }
}
