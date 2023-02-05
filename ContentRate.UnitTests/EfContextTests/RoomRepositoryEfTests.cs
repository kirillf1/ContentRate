using ContentRate.Infrastructure.Repositories.EfRepositories;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.UnitTests.EfContextTests
{
    public class RoomRepositoryEfTests
    {
        [Fact]
        public async Task AddRoom_NewRoom_ShouldEqualDomainRoom()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            IRoomRepository roomRepository = new RoomRepositoryEf(context);
            var contentCount = 10;
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), contentCount);
            var assessor = room.Assessors.First();
            context.Users.Add(new Infrastructure.Models.UserModel { Name = assessor.Name, Id = assessor.Id, Password = "fafsafafa" });
            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();
            await roomRepository.AddRoom(room);
            await roomRepository.SaveChanges();
            context.ChangeTracker.Clear();
            var roomFromDb = context.Rooms
                .Include(c => c.Users)
                .Include(c => c.ContentList)
                .First(c => c.Id == room.Id);

            Assert.True(roomFromDb.ContentList.Count == contentCount);
            Assert.True(roomFromDb.Users.Count == room.Assessors.Count);
            Assert.Equal(roomFromDb.Name, room.Name);
        }
        [Fact]
        public async Task UpdateRoom_NewNameAndPassword_RoomModelUpdatedInDB()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            IRoomRepository roomRepository = new RoomRepositoryEf(context);
            var users = await UserFactory.AddUserModelsToContext(context, 4, false);
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10, users.First().Id);
            await roomRepository.AddRoom(room);
            await roomRepository.SaveChanges();

            room.Name = "Test";
            room.RoomDetails.Password = "newpassword";
            await roomRepository.UpdateRoom(room);
            await roomRepository.SaveChanges();

            var roomFromDb = context.Rooms.First(c => c.Id == room.Id);
            Assert.Equal(room.Name, roomFromDb.Name);
            Assert.Equal(room.RoomDetails.Password, roomFromDb.RoomDetails.Password);
        }
        [Fact]
        public async Task UpdateRoom_NewContent_RoomModelUpdatedInDB()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            IRoomRepository roomRepository = new RoomRepositoryEf(context);
            var users = await UserFactory.AddUserModelsToContext(context, 4, false);
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10, users.First().Id);
            await roomRepository.AddRoom(room);
            await roomRepository.SaveChanges();
            var contentToAdd = new Content(Guid.NewGuid(), "newContent", ContentType.Audio, "t",
                users.Select(u => new Rating(u.Id, 10)));

            room.RemoveContent(room.ContentList.Take(2));
            room.AddContent(contentToAdd);
            await roomRepository.UpdateRoom(room);
            await roomRepository.SaveChanges();

            var roomFromDb = context.Rooms.Include(c => c.ContentList).First(c => c.Id == room.Id);
            Assert.Equal(roomFromDb.ContentList.Count, room.ContentList.Count);
            Assert.Contains(roomFromDb.ContentList, c => c.Name == contentToAdd.Name
            && c.Id == contentToAdd.Id
            && c.Ratings.Count == contentToAdd.Ratings.Count);
        }

        [Fact]
        public async Task UpdateRoom_UsersJoin_RoomModelUpdatedInDB()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            IRoomRepository roomRepository = new RoomRepositoryEf(context);
            var users = await UserFactory.AddUserModelsToContext(context, 4, false);
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10, users.First().Id);
            await roomRepository.AddRoom(room);
            await roomRepository.SaveChanges();

            foreach (var assessor in users.Select(c => new Assessor(c.Id, c.Name, c.IsMockUser)))
            {
                room.AssessorJoin(assessor);
            }
            await roomRepository.UpdateRoom(room);
            await roomRepository.SaveChanges();

            var roomFromDb = context.Rooms.Include(c => c.Users).Include(c => c.ContentList).First(c => c.Id == room.Id);
            Assert.Equal(roomFromDb.Users.Count, room.Assessors.Count);
            Assert.Equal(roomFromDb.ContentList.Sum(c => c.Ratings.Count), room.ContentList.Sum(c => c.Ratings.Count));
        }
        [Fact]
        public async Task DeleteRoom_ExistingRoom_RoomModelShouldDeletedWithContent()
        {
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            IRoomRepository roomRepository = new RoomRepositoryEf(context);
            var users = await UserFactory.AddUserModelsToContext(context, 4, false);
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10, users.First().Id);
            await roomRepository.AddRoom(room);
            await roomRepository.SaveChanges();


            await roomRepository.DeleteRoom(room.Id);
            await roomRepository.SaveChanges();


            Assert.Null(context.Rooms.FirstOrDefault(c => c.Id == room.Id));
            Assert.False(context.Content.Any(c => c.RoomId == room.Id));
            foreach (var user in users)
            {
                Assert.True(context.Users.Any(c=>c.Id == user.Id));
            }
        }

    }
}
