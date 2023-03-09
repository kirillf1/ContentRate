using ContentRate.Domain.Rooms;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Helpers;
using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.Infrastructure.Repositories.EfRepositories
{
    public class RoomRepositoryEf : IRoomRepository
    {
        private readonly ContentRateDbContext context;

        public RoomRepositoryEf(ContentRateDbContext context)
        {
            this.context = context;
        }
        public async Task AddRoom(Room room)
        {
            RoomModel roomModel = await CreateModelFromRoom(room);
            await context.AddAsync(roomModel);
        }

        private async Task<RoomModel> CreateModelFromRoom(Room room)
        {
            var roomModel = new RoomModel
            {
                ContentList = room.ContentList.Select(c => ContentConventer.ConvertContentToModel(c, room.Id)).ToList(),
                Id = room.Id,
                Name = room.Name,
                RoomDetails = new RoomDetailsModel
                {
                    CreationTime = room.RoomDetails.CreationTime,
                    CreatorId = room.RoomDetails.CreatorId,
                    IsPrivate = room.RoomDetails.IsPrivate,
                    Password = room.RoomDetails.Password
                },
                Users = await context.Users.Where(c => room.Assessors
                .Select(c => c.Id)
                .Contains(c.Id))
                .ToListAsync(),             
            };
            //var newUsers = room.Assessors.Where(c => !roomModel.Users.Any(r => c.Id == r.Id));
            //foreach (var newUser in newUsers)
            //{
            //    var user = new UserModel
            //    {
            //        Id = newUser.Id,
            //        IsMockUser = newUser.IsMockAssessor,
            //        Name = newUser.Name,
            //        Password = Guid.NewGuid().ToString(),
            //    };
            //    context.Users.Add(user);
            //    roomModel.Users.Add(user);
            //}
            return roomModel;
        }

        public async Task DeleteRoom(Guid id)
        {
            var roomModel = context.Rooms.Local.FirstOrDefault(r => r.Id == id)
               ?? await context.Rooms.SingleAsync(r => r.Id == id);
            context.Rooms.Remove(roomModel);
        }

        public async Task<Room> GetRoomById(Guid id)
        {
            // выполняем запрос всех данных без селекта чтобы затречить,
            // так как подозреваем, что комната будет меняться и чтобы не грузить еще раз,
            //выполняем это заранее
            var roomModel = await context.Rooms.Include(c => c.Users)
                .Include(c => c.ContentList)
                .SingleAsync(c => c.Id == id);
            return CreateRoomFromModel(roomModel);
        }

        public async Task<RoomDetails> GetRoomDetailsById(Guid id)
        {
            return await context.Rooms.Where(c => c.Id == id)
                .Select(r => new RoomDetails(r.RoomDetails.CreatorId, r.RoomDetails.IsPrivate, r.RoomDetails.Password)
                {
                    CreationTime = r.RoomDetails.CreationTime,
                }).SingleAsync();
        }

        public async Task<IEnumerable<Room>> GetRooms(RoomSearchCreteria roomSearch)
        {
            var query = FindRooms(roomSearch);
            return await query
                .Select(r => CreateRoomFromModel(r))
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsWithoutContent(RoomSearchCreteria roomSearch)
        {
            var query = FindRooms(roomSearch);
            return await query.Include(c => c.ContentList)
                .Select(r => CreateRoomFromModel(r))
                .ToListAsync();
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRoom(Room room)
        {
            var roomModel = context.Rooms.Local.FirstOrDefault(r => r.Id == room.Id)
                ?? await context.Rooms.Include(c => c.ContentList)
                .Include(c => c.Users)
                .SingleAsync(r => r.Id == room.Id);
            roomModel.Name = room.Name;
            roomModel.RoomDetails.IsPrivate = room.RoomDetails.IsPrivate;
            roomModel.RoomDetails.Password = room.RoomDetails.Password;
            UpdateContent(room, roomModel);
            await UpdateUsers(room, roomModel);
            context.Rooms.Update(roomModel);
        }
        private static Room CreateRoomFromModel(RoomModel roomModel)
        {
            return new Room(roomModel.Id, roomModel.Name,
                            new RoomDetails(roomModel.RoomDetails.CreatorId,
                              roomModel.RoomDetails.IsPrivate, roomModel.RoomDetails.Password)
                            {
                                CreationTime = roomModel.RoomDetails.CreationTime
                            },
                            roomModel.Users.Select(u => new Assessor(u.Id, u.Name, u.IsMockUser)),
                            roomModel.ContentList.Select(c => ContentConventer.ConvertModelToContent(c)));
        }

        private IQueryable<RoomModel> FindRooms(RoomSearchCreteria roomSearch)
        {
            var query = context.Rooms.Include(c => c.Users).AsQueryable();
            if (roomSearch.ByUserId.HasValue)
                query = query.Where(c => c.Users.Select(c => c.Id).Contains(roomSearch.ByUserId.Value));
            if (!roomSearch.IncludePrivateRooms.HasValue || roomSearch.IncludePrivateRooms == false)
                query = query.Where(c => !c.RoomDetails.IsPrivate);
            if (roomSearch.SkipCount.HasValue)
                query = query.Skip(roomSearch.SkipCount.Value);
            if (roomSearch.TakeCount.HasValue)
                query = query.Take(roomSearch.TakeCount.Value);
            return query;
        }
        private async Task UpdateUsers(Room room, RoomModel roomModel)
        {
            foreach (var user in roomModel.Users
               .Where(c => !room.Assessors.Any(n => n.Id == c.Id)))
            {
                roomModel.Users.Remove(user);
            }
            var newUserIds = room.Assessors
              .Where(c => !roomModel.Users.Any(n => n.Id == c.Id)).Select(c => c.Id).ToList();
            roomModel.Users.AddRange(await context.Users.Where(u => newUserIds.Contains(u.Id)).ToArrayAsync());

        }
        private void UpdateContent(Room room, RoomModel roomModel)
        {
            foreach (var contentModel in roomModel.ContentList
                .Where(c => !room.ContentList.Any(n => n.Id == c.Id)).ToList())
            {
                roomModel.ContentList.Remove(contentModel);
                context.Content.Remove(contentModel);
            }
            var contentToAdd = room.ContentList.Where(c => !roomModel.ContentList.Any(n => n.Id == c.Id)).ToList();
            foreach (var newContent in contentToAdd)
            {
                var contentModel = ContentConventer.ConvertContentToModel(newContent, roomModel.Id);
                roomModel.ContentList.Add(contentModel);
                context.Content.Add(contentModel);
            }
            foreach (var contentUpdate in room.ContentList.Where(c => !contentToAdd.Select(c => c.Id).Contains(c.Id)))
            {
                var contentModel = roomModel.ContentList.First(c => c.Id == contentUpdate.Id);
                contentModel.ContentType = contentUpdate.ContentType;
                contentModel.Path = contentUpdate.Path;
                ContentConventer.UpdateContentModel(contentModel, contentUpdate);
                context.Content.Update(contentModel);

            }
        }


    }
}
