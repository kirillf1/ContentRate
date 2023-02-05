using Ardalis.Result;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.Protos;
using RoomService = ContentRate.Protos.RoomService;

namespace ContentRate.GrpcClient.Rooms
{
    public class RoomClientGrpcService : IRoomService
    {
        private readonly RoomService.RoomServiceClient client;

        public RoomClientGrpcService(RoomService.RoomServiceClient client)
        {
            this.client = client;
        }
        public async Task<Result> CreateRoom(RoomUpdate roomCreate)
        {
            try
            {
                await client.CreateRoomAsync(RoomConverter.CreateRoomUpdateGrpc(roomCreate));
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }

        public async Task<Result> DeleteRoom(Guid roomId)
        {
            try
            {
                await client.DeleteRoomAsync(new RoomDeleteGrpc { Id = roomId.ToString() });
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }

        public async Task<Result<RoomEstimate>> JoinRoom(RoomEnter enter)
        {
            try
            {
                var room = await client.JoinRoomAsync(RoomConverter.ConvertRoomEnterToGrpc(enter));
                return RoomConverter.ConvertGrpcToRoomEstimate(room);
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }

        public async Task<Result<RoomUpdate>> OpenRoomToUpdate(Guid roomId)
        {
            try
            {
                var room = await client.OpenRoomToUpdateAsync(new RoomIdGrpc { Id = roomId.ToString() });
                return RoomConverter.ConvertGrpcToRoomUpdate(room);
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }

        public async Task<Result> UpdateRoom(RoomUpdate roomUpdate)
        {
            try
            {
                await client.UpdateRoomAsync(RoomConverter.CreateRoomUpdateGrpc(roomUpdate));
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }
    }
}
