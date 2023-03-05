using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Rooms;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.GrpcService.GrpcServices.Events;
using ContentRate.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Text.Json;

namespace ContentRate.GrpcService.GrpcServices.Rooms
{
    public class RoomServiceGrpc : Protos.RoomService.RoomServiceBase
    {
        private readonly IRoomService roomService;
        private readonly RoomEstimateNotifier notifier;

        public RoomServiceGrpc(IRoomService roomService, RoomEstimateNotifier notifier)
        {
            this.roomService = roomService;
            this.notifier = notifier;
        }
        public override async Task<Empty> DeleteRoom(RoomDeleteGrpc request, ServerCallContext context)
        {
            var result = await roomService.DeleteRoom(Guid.Parse(request.Id));
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return new Empty();
        }
        public async override Task<Empty> CreateRoom(RoomUpdateGrpc request, ServerCallContext context)
        {
            var roomUpdate = RoomConverter.ConvertGrpcToRoomUpdate(request);
            var result = await roomService.CreateRoom(roomUpdate);
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return new Empty();
        }
        public override async Task<RoomEstimateGrpc> JoinRoom(RoomEnterGrpc request, ServerCallContext context)
        {
            var roomEnter = new RoomEnter
            {
                Password = request.Password,
                RoomId = Guid.Parse(request.RoomId),
                AssessorId = Guid.Parse(request.UserId)
            };
            var result = await roomService.JoinRoom(roomEnter);
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            var room = result.Value;
            // нужно создать отдельный объект для события, а не костыль делать
            await notifier.NotifyAllRoom(roomEnter.RoomId, new RoomEstimateEventGrpc
            {
                EventType = EventType.AssessorJoined,
                JsonBody = JsonSerializer.Serialize(new UserTitle
                {
                    Id = roomEnter.AssessorId,
                    Name = ""
                })
            }, roomEnter.AssessorId); 
            RoomEstimateGrpc roomEstimateGrpc = RoomConverter.CreateRoomEstimateGrpc(room);
            return roomEstimateGrpc;
        }
        public override async Task<Empty> UpdateRoom(RoomUpdateGrpc request, ServerCallContext context)
        {
            var roomUpdate = RoomConverter.ConvertGrpcToRoomUpdate(request);
            var result = await roomService.UpdateRoom(roomUpdate);
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return new Empty();
        }
        public override async Task<RoomUpdateGrpc> OpenRoomToUpdate(RoomIdGrpc request, ServerCallContext context)
        {
            var result = await roomService.OpenRoomToUpdate(Guid.Parse(request.Id));
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            return RoomConverter.CreateRoomUpdateGrpc(result);
        }
        
    }
}
