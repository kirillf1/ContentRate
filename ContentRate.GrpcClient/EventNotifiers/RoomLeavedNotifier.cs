using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Events;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal class RoomLeavedListener : EventNotifier<RoomExit>
    {
        public RoomLeavedListener(ContentRateEventHandler<RoomExit> handler) : base(handler)
        {
        }
    }
}
