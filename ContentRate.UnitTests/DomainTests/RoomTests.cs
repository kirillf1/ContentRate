using ContentRate.UnitTests.Helpers;

namespace ContentRate.UnitTests.DomainTests
{
    public class RoomTests
    {
        [Fact]
        public void AssessorJoin_NewUser_AddedNewUserInRoom()
        {
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10);
            var newAssessor = AssessorFactory.CreateAssessor();

            room.AssessorJoin(newAssessor);

            Assert.Contains(newAssessor, room.Assessors);
            Assert.True(room.ContentList.All(c => c.Ratings.Any(c => c.AssessorId == newAssessor.Id)));
        }
        [Fact]
        public void AssessorLeave_OldUser_UserRemovedWithContentRating()
        {
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10);
            var assessorForLeave = AssessorFactory.CreateAssessor();
            room.AssessorJoin(assessorForLeave);

            room.AssessorLeave(assessorForLeave.Id);

            Assert.DoesNotContain(assessorForLeave, room.Assessors);
            Assert.True(room.ContentList.All(c => !c.Ratings.Any(c => c.AssessorId == assessorForLeave.Id)));
        }
        [Fact]
        public void LeaveRoom_Creator_ThrowArgumentEx()
        {
            var userIdForLeave = Guid.NewGuid();
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10, userIdForLeave);

            Assert.Throws<ArgumentException>(() => room.AssessorLeave(userIdForLeave));
        }

        [Fact]
        public void AddContentRange_NewContent_NewRatingForAllUsers()
        {
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 0);
            room.AssessorJoin(AssessorFactory.CreateAssessor());
            var content = Enumerable.Repeat(() => ContentFactory.CreateContent(Guid.NewGuid()), 10);

            room.AddContentRange(content.Select(c => c.Invoke()));

            Assert.Equal(10, room.ContentList.Count);
            Assert.True(room.ContentList.All(c => c.Ratings.Count == room.Assessors.Count
            || c.Ratings.Any(d => room.Assessors.Any(u=> u.Id == d.AssessorId))));
        }
        [Fact]
        public void JoinRoom_UserExists_NotAddSameUser()
        {
            var room = RoomFactory.CreateRoom(Guid.NewGuid(), 10);
            var newAssessor = AssessorFactory.CreateAssessor();

            room.AssessorJoin(newAssessor);
            room.AssessorJoin(newAssessor);

            Assert.Contains(newAssessor, room.Assessors);
            Assert.True(room.Assessors.Count == 2);

        }
    }
}
