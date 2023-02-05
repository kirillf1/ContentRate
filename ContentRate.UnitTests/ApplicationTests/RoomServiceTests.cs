using ContentRate.Application.Contracts.Content;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Rooms;
using ContentRate.Infrastructure.Repositories.EfRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentRate.UnitTests.ApplicationTests
{
    public class RoomServiceTests
    {
        [Fact]
        public async Task UpdateRoom_ContentAndAssessorsChanged_ShouldUpdateInRepository()
        {
            var context = ContentRateDbContextFactory.CreateRateContextInMemory();
            var roomRepository = new RoomRepositoryEf(context);
            var userRepository = new UserRepositoryEf(context);
            var user = new User(Guid.NewGuid(),"testUser","1222222");
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 20,user.Id);
            await userRepository.AddUser(user);
            await userRepository.SaveChanges();
            await roomRepository.AddRoom(room);
            await roomRepository.SaveChanges();
            IRoomService roomService = new RoomService(roomRepository, userRepository);

            var roomUpdate = ConvertRoomToUpdateRoom(room);
            roomUpdate.Assessors.Add(new AssessorTitle { Id = Guid.NewGuid(), Name = "NewTestAssessor", IsMock = true });
            roomUpdate.Content.RemoveRange(0, 2);
            var newContent = new ContentDetails { Id = Guid.NewGuid(), ContentType = ContentType.Video, Name = "testContent", Path = "testPath" };
            roomUpdate.Content.Add(newContent);
            var result = await roomService.UpdateRoom(roomUpdate);

            var roomFromRepository = await roomRepository.GetRoomById(room.Id);
            Assert.True(result.IsSuccess);
            Assert.True(roomUpdate.Assessors.Count == roomFromRepository.Assessors.Count);
            Assert.True(roomUpdate.Content.Count == roomFromRepository.ContentList.Count);
            Assert.Contains(roomFromRepository.ContentList, c => c.Id == newContent.Id);
        }
        private static RoomUpdate ConvertRoomToUpdateRoom(Room room)
        {
            return new RoomUpdate
            {
                Assessors = room.Assessors.Select(a => new AssessorTitle
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsMock = a.IsMockAssessor
                }).ToList(),
                Content = room.ContentList.Select(c => new ContentDetails
                {
                    
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Id = c.Id,
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
    }
}
