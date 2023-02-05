using Ardalis.Result;
using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Domain.Rooms;
using ContentRate.Domain.Users;

namespace ContentRate.Application.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly IUserRepository userRepository;

        public RoomService(IRoomRepository roomRepository, IUserRepository userRepository)
        {
            this.roomRepository = roomRepository;
            this.userRepository = userRepository;
        }
        public async Task<Result> CreateRoom(RoomUpdate roomCreate)
        {
            try
            {
                Room room = ConvertRoomUpdateToRoom(roomCreate);
                await roomRepository.AddRoom(room);
                await roomRepository.SaveChanges();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        public async Task<Result> DeleteRoom(Guid roomId)
        {
            try
            {
                await roomRepository.DeleteRoom(roomId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result<RoomEstimate>> JoinRoom(RoomEnter enter)
        {
            try
            {
                var room = await roomRepository.GetRoomById(enter.RoomId);
                if (room.RoomDetails.Password is not null && room.RoomDetails.Password != enter.Password)
                    return Result.Forbidden();
                var user = await userRepository.GetUserById(enter.AssessorId);
                room.AssessorJoin(new Assessor(user.Id, user.Name, user.IsMockUser));
                await roomRepository.UpdateRoom(room);
                await roomRepository.SaveChanges();
                var roomEstimate = ConvertRoomToEstimateRoom(room);
                return Result.Success(roomEstimate);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }

        private static RoomEstimate ConvertRoomToEstimateRoom(Room room)
        {
            return new RoomEstimate
            {
                Assessors = room.Assessors.Select(a => new Contracts.Rooms.AssessorTitle
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsMock = a.IsMockAssessor
                }).ToList(),
                Content = room.ContentList.Select(c => new ContentDetails
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Path = c.Path,
                    Ratings = c.Ratings.Select(r => new ContentRating { Value = r.Value, AssessorId = r.AssessorId }).ToList()
                }).ToList(),
                CreatorId = room.RoomDetails.CreatorId,
                Id = room.Id,
                Name = room.Name,
            };
        }

        public async Task<Result> UpdateRoom(RoomUpdate roomUpdate)
        {
            try
            {
                var room = await roomRepository.GetRoomById(roomUpdate.Id);
                room.Name = roomUpdate.Name;
                room.RoomDetails.Password = roomUpdate.Password;
                room.RoomDetails.IsPrivate = roomUpdate.IsPrivate;

                var assessorForUpdate = GetAssessorFromRoomUpdate(roomUpdate).ToList();
                var assessorForDelete = room.Assessors.Where(a => !assessorForUpdate.Any(c => c.Id == a.Id));
                foreach (var assessor in assessorForDelete) 
                {
                    room.AssessorLeave(assessor.Id);
                    if(assessor.IsMockAssessor)
                        await userRepository.DeleteUser(assessor.Id);
                }
                    
                var assessorForAdd = assessorForUpdate.Where(a => !room.Assessors.Any(c => a.Id == c.Id));
                // тут надо учесть, что возможно стоит добавить событие для пользователей, т.к. изменения
                // могут происходить когда люди уже оценивают
                foreach (var assessor in assessorForAdd)
                {
                    if (assessor.IsMockAssessor)
                        await userRepository.AddUser(new User(assessor.Id, assessor.Name, Guid.NewGuid().ToString()));
                    room.AssessorJoin(assessor);
                }
                await userRepository.SaveChanges();
                var contentForUpdate = GetContentFromRoomUpdate(roomUpdate).ToList();
                var contentForDelete = room.ContentList.Where(c => !contentForUpdate.Any(n => n.Id == c.Id)).ToList();
                room.RemoveContent(contentForDelete);
                var contentForAdd = contentForUpdate.Where(c => !room.ContentList.Any(n => n.Id == c.Id)).ToList();
                foreach (var content in contentForAdd)
                    room.AddContent(content);

                await roomRepository.UpdateRoom(room);
                await roomRepository.SaveChanges();
                return Result.Success();

            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        private static Room ConvertRoomUpdateToRoom(RoomUpdate roomCreate)
        {
            var roomDetails = new RoomDetails(roomCreate.CreatorId, roomCreate.IsPrivate, roomCreate.Password);
            IEnumerable<Assessor> assessors = GetAssessorFromRoomUpdate(roomCreate);
            IEnumerable<Content> content = GetContentFromRoomUpdate(roomCreate);
            var room = new Room(roomCreate.Id, roomCreate.Name, roomDetails, assessors, content);
            return room;
        }

        public async Task<Result<RoomUpdate>> OpenRoomToUpdate(Guid roomId)
        {
            try
            {
                var room = await roomRepository.GetRoomById(roomId);
                return ConvertRoomToUpdateRoom(room);
            }
            catch(Exception ex)
            {
                return Result.Error(ex.Message);
            }
        }
        private static RoomUpdate ConvertRoomToUpdateRoom(Room room)
        {
            return new RoomUpdate
            {
                Assessors = room.Assessors.Select(a => new Contracts.Rooms.AssessorTitle
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsMock = a.IsMockAssessor
                }).ToList(),
                Content = room.ContentList.Select(c => new ContentDetails
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Path = c.Path,
                    Ratings = c.Ratings.Select(r => new ContentRating { Value = r.Value, AssessorId = r.AssessorId }).ToList()
                }).ToList(),
                CreatorId = room.RoomDetails.CreatorId,
                Id = room.Id,
                Name = room.Name,
                IsPrivate = room.RoomDetails.IsPrivate,
                Password = room.RoomDetails.Password,
            };
        }
        private static IEnumerable<Assessor> GetAssessorFromRoomUpdate(RoomUpdate roomCreate)
        {
            return roomCreate.Assessors.Select(c => new Assessor(c.Id, c.Name, c.IsMock));
        }

        private static IEnumerable<Content> GetContentFromRoomUpdate(RoomUpdate roomCreate)
        {
            return roomCreate.Content.Select(c => new Content(c.Id, c.Name, c.ContentType, c.Path,
                            c.Ratings.Select(r => new Rating(r.AssessorId, r.Value))));
        }

    }
}
