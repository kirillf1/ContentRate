using Ardalis.Result;
using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;

namespace ContentRate.Application.Rooms
{
    public interface IRoomEstimationService
    {
        public Task<Result> LeaveRoom(RoomExit exit);
        public Task<Result> EstimateContent(ContentEstimate contentEstimate);
        public Task<Result> EndEstimation(Guid roomId);
    }
}
