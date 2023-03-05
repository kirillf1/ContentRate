using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Events;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal class RoomJoinedListener : EventNotifier<UserTitle>
    {
        public RoomJoinedListener(ContentRateEventHandler<UserTitle> handler) : base(handler)
        {
        }
    }
}
