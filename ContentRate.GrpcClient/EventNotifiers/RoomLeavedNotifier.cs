using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Events;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal class RoomLeavedNotifier : EventNotifier<RoomExit>
    {
        public RoomLeavedNotifier(ContentRateEventHandler<RoomExit> handler) : base(handler)
        {
        }
    }
}
