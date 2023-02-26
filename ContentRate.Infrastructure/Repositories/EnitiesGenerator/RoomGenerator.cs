using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Repositories.EnitiesGenerator.Factories;

namespace ContentRate.Infrastructure.Repositories.EnitiesGenerator
{
    public class RoomGenerator : IRoomRepository
    {
        public RoomGenerator()
        {

        }

        private static List<Room> rooms = new();
        public async Task AddRoom(Room room)
        {
            await UpdateRoom(room);
        }

        public Task DeleteRoom(Guid id)
        {
            if (rooms.Any(c => c.Id == id))
                rooms.Remove(rooms.First(c => c.Id == id));
            return Task.CompletedTask;
        }

        public Task<Room> GetRoomById(Guid id)
        {
            if (!rooms.Any(c => c.Id == id))
                return Task.FromResult(RoomFactory.CreateRoom(id, 500));
            return Task.FromResult(rooms.First(c => c.Id == id));
        }

        public Task<RoomDetails> GetRoomDetailsById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Room>> GetRooms(RoomSearchCreteria roomSearch)
        {
            if (rooms.Count == 0)
                rooms = new List<Room>();
            for (int i = 0; i < 100; i++)
            {
                rooms.Add(RoomFactory.CreateRoom(Guid.NewGuid(), 500, Guid.NewGuid(), false));
            }
            return Task.FromResult(rooms.AsEnumerable());
        }

        public Task<IEnumerable<Room>> GetRoomsWithoutContent(RoomSearchCreteria roomSearch)
        {
            if (rooms.Count == 0)
                rooms = new List<Room>();
            for (int i = 0; i < 100; i++)
            {
                rooms.Add(RoomFactory.CreateRoom(Guid.NewGuid(), 500, Guid.NewGuid(), false));
            }
            return Task.FromResult(rooms.AsEnumerable());
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task UpdateRoom(Room room)
        {
            if (rooms.Any(c => c.Id == room.Id))
                rooms.Remove(rooms.First(c => c.Id == room.Id));
            rooms.Add(room);
            return Task.CompletedTask;
        }
    }
}
