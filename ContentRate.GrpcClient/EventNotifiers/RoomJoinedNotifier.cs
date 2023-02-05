using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Events;

namespace ContentRate.GrpcClient.EventNotifiers
{
    internal class RoomJoinedNotifier : EventNotifier<UserTitle>
    {
        public RoomJoinedNotifier(ContentRateEventHandler<UserTitle> handler) : base(handler)
        {
        }
    }
}
