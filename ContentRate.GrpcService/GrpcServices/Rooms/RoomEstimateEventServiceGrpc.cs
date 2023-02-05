using ContentRate.GrpcService.GrpcServices.Events;
using ContentRate.Protos;
using Grpc.Core;

namespace ContentRate.GrpcService.GrpcServices.Rooms
{
    public class RoomEstimateEventServiceGrpc : RoomEstimateEventService.RoomEstimateEventServiceBase
    {
        private readonly RoomEstimateEventListenerStorage storage;
        public RoomEstimateEventServiceGrpc(RoomEstimateEventListenerStorage storage)
        {
            this.storage = storage;
        }
        public override async Task MonitorEvents(SubscriberGrpc request, IServerStreamWriter<RoomEstimateEventGrpc> responseStream, ServerCallContext context)
        {
            var key = new EstimateStorageKey(Guid.Parse(request.RoomId), Guid.Parse(request.AssesorId));
            try
            {
                if (!storage.TryAddConnection(key, responseStream))
                    throw new RpcException(new Status(StatusCode.Aborted,"Can't subscribe to events"));
                await Task.Delay(-1, context.CancellationToken);

            }
            finally
            {
                storage.TryRemoveConnection(key);
            }
        }
    }
}
