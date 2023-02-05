namespace ContentRate.Domain.Rooms
{
    public record RoomSearchCreteria(bool? IncludePrivateRooms = null,int? SkipCount = null, int? TakeCount = null,
        Guid? ByUserId = null);
    public interface IRoomRepository
    {
        public Task<IEnumerable<Room>> GetRooms(RoomSearchCreteria roomSearch);
        public Task<IEnumerable<Room>> GetRoomsWithoutContent(RoomSearchCreteria roomSearch);
        public Task<Room> GetRoomById(Guid id);
        public Task<RoomDetails> GetRoomDetailsById(Guid id);
        public Task UpdateRoom(Room room);
        public Task AddRoom(Room room);
        public Task DeleteRoom(Guid id);
        public Task SaveChanges(CancellationToken cancellationToken = default);

    }
}
