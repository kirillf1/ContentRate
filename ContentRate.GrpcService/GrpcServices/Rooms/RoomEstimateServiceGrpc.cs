using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Rooms;
using ContentRate.GrpcExtensions.Helpers;
using ContentRate.GrpcService.GrpcServices.Events;
using ContentRate.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Text.Json;

namespace ContentRate.GrpcService.GrpcServices.Rooms
{
    public class RoomEstimateServiceGrpc : RoomEstimateService.RoomEstimateServiceBase
    {
        private readonly IRoomEstimationService estimationService;
        private readonly RoomEstimateNotifier notifier;

        public RoomEstimateServiceGrpc(IRoomEstimationService estimationService, RoomEstimateNotifier notifier)
        {
            this.estimationService = estimationService;
            this.notifier = notifier;
        }
        public override async Task<Empty> EstimateContent(ContentEstimateGrpc request, ServerCallContext context)
        {
            ContentEstimate contentEstimate = ContentConverter.ConvertGrpcToContentEstimate(request);
            var result = await estimationService.EstimateContent(contentEstimate);
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            await NotifyRoom(contentEstimate.RoomId, EventType.ContentEstimated, contentEstimate, contentEstimate.AssessorId);
            return new Empty();
        }

        public override Task<Empty> EndEstimation(RoomIdGrpc request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }
        public override async Task<Empty> LeaveRoom(RoomExitGrpc request, ServerCallContext context)
        {
            var roomExit = new Application.Contracts.Rooms.RoomExit
            {
                RoomId = Guid.Parse(request.RoomId),
                AssessorId = Guid.Parse(request.UserId),
            };
            var result = await estimationService.LeaveRoom(roomExit);
            if (!result.IsSuccess)
                throw new RpcException(new Status(StatusCode.Unknown, $"Errors: {string.Join(',', result.Errors)}"));
            await NotifyRoom(roomExit.RoomId, EventType.AssessorLeaved, roomExit);
            return new Empty();
        }
        private async Task NotifyRoom<T>(Guid roomId,EventType eventType,T tEvent, Guid? sender = null)
        {
            var eventGrpc = new RoomEstimateEventGrpc
            {
                EventType = eventType,
                JsonBody = JsonSerializer.Serialize(tEvent)
            };
            await notifier.NotifyAllRoom(roomId, eventGrpc, sender);
        }
    }
}
