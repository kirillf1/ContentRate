using Ardalis.Result;
using ContentRate.Application.Contracts.Rooms;

namespace ContentRate.Application.Rooms
{
    public interface IRoomService
    {
        public Task<Result> CreateRoom(RoomUpdate roomCreate);
        public Task<Result> UpdateRoom(RoomUpdate roomUpdate);
        public Task<Result> DeleteRoom(Guid roomId);
        public Task<Result<RoomEstimate>> JoinRoom(RoomEnter enter);
        public Task<Result<RoomUpdate>> OpenRoomToUpdate(Guid roomId);
    }
}
