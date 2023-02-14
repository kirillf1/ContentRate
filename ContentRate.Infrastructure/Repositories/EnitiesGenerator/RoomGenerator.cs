using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Repositories.EnitiesGenerator.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.Infrastructure.Repositories.EnitiesGenerator
{
    public class RoomGenerator : IRoomRepository
    {
        public Task AddRoom(Room room)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoom(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Room> GetRoomById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RoomDetails> GetRoomDetailsById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Room>> GetRooms(RoomSearchCreteria roomSearch)
        {
            var rooms = new List<Room>();
            for (int i = 0; i < 100; i++)
            {
                rooms.Add(RoomFactory.CreateRoom(Guid.NewGuid(), 20, Guid.NewGuid(), false));
            }
            return Task.FromResult(rooms.AsEnumerable());
        }

        public Task<IEnumerable<Room>> GetRoomsWithoutContent(RoomSearchCreteria roomSearch)
        {
            var rooms = new List<Room>();
            for (int i = 0; i < 100; i++)
            {
                rooms.Add(RoomFactory.CreateRoom(Guid.NewGuid(), 20, Guid.NewGuid(), false));
            }
            return Task.FromResult(rooms.AsEnumerable());
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoom(Room room)
        {
            throw new NotImplementedException();
        }
    }
}
