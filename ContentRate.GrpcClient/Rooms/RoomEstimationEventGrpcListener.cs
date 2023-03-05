using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Events;
using ContentRate.Application.Rooms;
using ContentRate.GrpcClient.EventNotifiers;
using ContentRate.Protos;
using Grpc.Core;

namespace ContentRate.GrpcClient.Rooms
{
    public class RoomEstimationEventGrpcListener : IRoomEstimationEventListener
    {
        private readonly Dictionary<EventType, Func<IEventListenerBase>> eventListener;

        public event ContentRateEventHandler<ContentEstimate> ContentEstimated;
        public event ContentRateEventHandler<RoomExit> AssessorLeaved;
        public event ContentRateEventHandler<UserTitle> AssessorJoined;
        private readonly RoomEstimateEventService.RoomEstimateEventServiceClient client;
        CancellationTokenSource tokenSource;
        public RoomEstimationEventGrpcListener(RoomEstimateEventService.RoomEstimateEventServiceClient client)
        {
            this.client = client;
            tokenSource = new CancellationTokenSource();

            eventListener = new Dictionary<EventType, Func<IEventListenerBase>>
        {
            {EventType.AssessorLeaved, ()=> new RoomLeavedListener(AssessorLeaved)},
            {EventType.AssessorJoined, ()=> new RoomJoinedListener(AssessorJoined)},
            {EventType.ContentEstimated,()=> new ContentEstimatedListener(ContentEstimated)},
            // добавить EstimationEnd
        };
        }


        public Task StopListenEvents()
        {
            tokenSource.Cancel();
            return Task.CompletedTask;
        }
        // подумать над контролем прослушки (нужно чтобы несколько подключений не было одновременно)
        public async Task StartListenEvents(Guid assessorId, Guid roomId)
        {
            tokenSource = new CancellationTokenSource();
            var listner = client.MonitorEvents(new SubscriberGrpc
            {
                AssesorId = assessorId.ToString(),
                RoomId = roomId.ToString(),
            });
            if (tokenSource.TryReset())
            {
                await foreach (var _event in listner.ResponseStream.ReadAllAsync(tokenSource.Token))
                {
                    var isNotified = eventListener[_event.EventType]
                        .Invoke()
                        .TryNotify(_event);
                }
            }
        }

      
        
    }
}
