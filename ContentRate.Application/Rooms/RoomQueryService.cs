using Ardalis.Result;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Domain.Rooms;

namespace ContentRate.Application.Rooms
{
    // Можно данный сервер для оптимизации переделать, чтобы запросы проходили через EfCore
    // Но все равно нужно выделить абстракцию DbContext
    public class RoomQueryService : IRoomQueryService
    {
        private readonly IRoomRepository roomRepository;

        public RoomQueryService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task<Result<IEnumerable<RoomTitle>>> GetPersonalRoomTitles(Guid userId)
        {
            try
            {
                var rooms = await roomRepository.GetRoomsWithoutContent(new RoomSearchCreteria(IncludePrivateRooms: true, ByUserId: userId));
                return ConvertRoomsToRoomTitles(rooms);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }

        }

        public async Task<Result<IEnumerable<RoomTitle>>> GetRoomTitles()
        {
            try
            {
                var rooms = await roomRepository.GetRoomsWithoutContent(new RoomSearchCreteria());
                return ConvertRoomsToRoomTitles(rooms);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        private static Result<IEnumerable<RoomTitle>> ConvertRoomsToRoomTitles(IEnumerable<Room> rooms)
        {
            if (!rooms.Any())
                return Result.NotFound();
            var roomTitles = rooms.Select(room => new RoomTitle
            {
                AssessorCount = room.Assessors.Count,
                CreationTime = room.RoomDetails.CreationTime,
                Creator = new Contracts.Users.UserTitle
                {
                    Id = room.RoomDetails.CreatorId,
                    Name = room.Assessors.Single(a => a.Id == room.RoomDetails.CreatorId).Name
                },
                Id = room.Id,
                Name = room.Name,
            }).ToList();
            return Result.Success(roomTitles.AsEnumerable());
        }

    }
}
