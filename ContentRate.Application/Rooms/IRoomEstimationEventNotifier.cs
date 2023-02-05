using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Events;

namespace ContentRate.Application.Rooms
{
    public interface IRoomEstimationEventNotifier
    {
        public Task StartListenEvents(Guid assessorId, Guid roomId);
        public Task StopListenEvents();
        public event ContentRateEventHandler<ContentEstimate> ContentEstimated;
        public event ContentRateEventHandler<RoomExit> AssessorLeaved;
        public event ContentRateEventHandler<UserTitle> AssessorJoined;
    }
}
