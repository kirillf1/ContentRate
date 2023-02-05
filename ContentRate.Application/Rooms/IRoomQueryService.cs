using Ardalis.Result;
using ContentRate.Application.Contracts.Rooms;

namespace ContentRate.Application.Rooms
{
    public interface IRoomQueryService
    {
        public Task<Result<IEnumerable<RoomTitle>>> GetRoomTitles();
        public Task<Result<IEnumerable<RoomTitle>>> GetPersonalRoomTitles(Guid userId);
    }
}
