using ContentRate.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ContentRate.UnitTests.EfContextTests
{
    public class AddEntityTests
    {
        [Fact]
        public void AddRoom_NewRoom_ShouldAddWithChildEntities()
        {
            var room = CreateRoomModel();
            var user = room.Users.First(c => room.RoomDetails.CreatorId == c.Id);
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());

            context.Add(room);
            context.Add(user);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            var roomFromDb = context.Rooms.Include(c=>c.Users).Include(c=>c.ContentList).First();
            var userFromDb = context.Users.First(c => c.Id == user.Id);
            Assert.Equal(room.Name, roomFromDb.Name);
            Assert.Equal(room.Users.Count, roomFromDb.Users.Count);
            Assert.Equal(room.ContentList.Count, roomFromDb.ContentList.Count);
            Assert.Equal(user.Name, userFromDb.Name);
            Assert.Equal(user.Password,userFromDb.Password);
        }
        [Fact]
        public void AddRoom_NewRoomWithoutAddUser_ShouldAddWithChildEntities()
        {
            var room = CreateRoomModel();
            var user = room.Users.First(c => room.RoomDetails.CreatorId == c.Id);
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());

            context.Add(room);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            var roomFromDb = context.Rooms.Include(c => c.Users).Include(c => c.ContentList).First();
            Assert.Equal(roomFromDb.Users.Count, context.Users.Count());
            Assert.True(context.Content.Any());
        }
        [Fact]
        public void RemoveRoom_RoomAddedInDb_ShouldRemoveContent()
        {
            var room = CreateRoomModel();
            var user = room.Users.First(c => room.RoomDetails.CreatorId == c.Id);
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            context.Add(room);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            var roomFromDb = context.Rooms.Include(c => c.Users).First();
            context.Rooms.Remove(roomFromDb);
            context.SaveChanges();

            Assert.True(!context.Rooms.Any());
            Assert.True(!context.Content.Any());
            Assert.True(context.Users.Count() == room.Users.Count);
        }
        [Fact]
        public void RemoveUser_RoomAddedInDb_ShouldSaveRoom()
        {
            var room = CreateRoomModel();
            var user = room.Users.First(c => room.RoomDetails.CreatorId == c.Id);
            using var context = ContentRateDbContextFactory.CreateRateContext(ContentRateDbContextFactory.DefaultConnection.Invoke());
            context.Add(room);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            var userFromDb = context.Users.First(c=>c.Id== user.Id);
            context.Users.Remove(userFromDb);
            context.SaveChanges();

            Assert.True(context.Rooms.Any());
            Assert.True(context.Content.Any());
            Assert.True(context.Users.Count() != room.Users.Count);
        }
        private static RoomModel CreateRoomModel()
        {
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                IsMockUser = true,
                Name = "testUser",
                Notifications = new(),
                Password = "password",
            };
            var room = new RoomModel()
            {
                ContentList = new List<ContentModel> {
                    new ContentModel
                    {
                    ContentType = ContentType.Video,
                    Id = Guid.NewGuid(),
                    Name = "test",
                    Path = "fasfa",
                    Ratings = new List<RatingModel>{new RatingModel { UserId= user.Id, Value = 10} }
                    }
                },
                Id = Guid.NewGuid(),
                Name = "testRoom",
                RoomDetails = new RoomDetailsModel { CreationTime = DateTime.Now, CreatorId = user.Id },
                Users = new List<UserModel> { user, new UserModel { Id = Guid.NewGuid(), Password = "t11111", Name = "test" } }
            };
            return room;
        }
    }
}
