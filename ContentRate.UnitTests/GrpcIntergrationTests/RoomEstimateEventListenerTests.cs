extern alias GrpcClient;
using ContentRate.Application.Contracts.Rooms;
using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Rooms;
using ContentRate.UnitTests.GrpcIntergrationTests.Helpers;
using Xunit.Abstractions;
using ContentRate.Protos;
using GrpcClient::ContentRate.GrpcClient.Rooms;

namespace ContentRate.UnitTests.GrpcIntergrationTests
{
    public class RoomEstimateEventListenerTests : IntegrationTestBase
    {
        public RoomEstimateEventListenerTests(GrpcTestFixture<StartupTest> fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {
        }
        List<UserTitle> UserTitles = new();
        RoomEstimate? roomForUpdate;
        // нужно отрефакторить
        [Fact]
        public async Task JoinRoom_TwoAssessors_ShouldOneJoinNotified()
        {
            var userFirst = await PreloadUser(Guid.NewGuid());
            var userSecond = await PreloadUser(Guid.NewGuid());
            var rooms = await PreloadRooms(userFirst.Id, 1, false);
            var room = rooms.First();
            var firstChannel = CreateChannel();
            var secondChannel = CreateChannel();
            IRoomService roomServiceFirst = new RoomClientGrpcService(new GrpcClient.ContentRate.Protos.RoomService.RoomServiceClient(firstChannel));
            IRoomService roomServiceSecond = new RoomClientGrpcService(new GrpcClient.ContentRate.Protos.RoomService.RoomServiceClient(secondChannel));
            IRoomEstimationService estimationClient = new RoomEstimationClientGrpcService(new GrpcClient.ContentRate.Protos.RoomEstimateService.RoomEstimateServiceClient(firstChannel));
            
            var notifier = new RoomEstimationEventGrpcNotifier(new GrpcClient.ContentRate.Protos.RoomEstimateEventService.RoomEstimateEventServiceClient(firstChannel));
            var result = await roomServiceFirst.JoinRoom(new RoomEnter { AssessorId = userFirst.Id, RoomId = room.Id });
            notifier.AssessorJoined += AssessorJoined;
            var notifierTask = notifier.StartListenEvents(userFirst.Id, room.Id);
            result = await roomServiceSecond.JoinRoom(new RoomEnter { AssessorId = userSecond.Id, RoomId = room.Id });
            await notifier.StopListenEvents();

            Assert.True(result.IsSuccess);
            // wait for events
            if (!UserTitles.Any(c => c.Id == userSecond.Id))
                await Task.Delay(500);
            Assert.Contains(UserTitles, c => c.Id == userSecond.Id);
            Assert.True(notifierTask.IsFaulted || notifierTask.IsCanceled);

        }
        [Fact]
        public async Task EstimateContent_SecondAssesorEstimate_ShouldNotifyFirst()
        {
            var userFirst = await PreloadUser(Guid.NewGuid());
            var userSecond = await PreloadUser(Guid.NewGuid());
            var rooms = await PreloadRooms(userFirst.Id, 1, false);
            var room = rooms.First();
            var contentId = room.ContentList.First().Id;
            var firstChannel = CreateChannel();
            var secondChannel = CreateChannel();
            double newRating = 9.25;
            IRoomService roomServiceFirst = new RoomClientGrpcService(new GrpcClient.ContentRate.Protos.RoomService.RoomServiceClient(firstChannel));
            IRoomService roomServiceSecond = new RoomClientGrpcService(new GrpcClient.ContentRate.Protos.RoomService.RoomServiceClient(secondChannel));
            IRoomEstimationService estimationClientFirst = new RoomEstimationClientGrpcService(new GrpcClient.ContentRate.Protos.RoomEstimateService.RoomEstimateServiceClient(firstChannel));
            IRoomEstimationService estimationClientSecond = new RoomEstimationClientGrpcService(new GrpcClient.ContentRate.Protos.RoomEstimateService.RoomEstimateServiceClient(secondChannel));
           
            var notifier = new RoomEstimationEventGrpcNotifier(new  GrpcClient.ContentRate.Protos.RoomEstimateEventService.RoomEstimateEventServiceClient(secondChannel));
            var resultRoom = await roomServiceFirst.JoinRoom(new RoomEnter { AssessorId = userFirst.Id, RoomId = room.Id });
            notifier.ContentEstimated += ContentEstimated;
            var notifierTask = notifier.StartListenEvents(userFirst.Id, room.Id);
            resultRoom = await roomServiceSecond.JoinRoom(new RoomEnter { AssessorId = userSecond.Id, RoomId = room.Id });
            await estimationClientSecond.EstimateContent(new Application.Contracts.Content.ContentEstimate
            { AssessorId = userSecond.Id, ContentId = contentId, NewValue = newRating, RoomId = room.Id }
            );
            roomForUpdate = resultRoom.Value;
            await notifier.StopListenEvents();
            
            // wait for events
            await Task.Delay(500);
            Assert.True(roomForUpdate.Content
                .First(c => c.Id == contentId)
                .Ratings
                .First(c => c.AssessorId == userSecond.Id).Value == newRating);
        }
        private Task AssessorJoined(Application.Contracts.Users.UserTitle param)
        {
            UserTitles.Add(param);
            return Task.CompletedTask;
        }
        private Task ContentEstimated(Application.Contracts.Content.ContentEstimate param)
        {
            var content = roomForUpdate?.Content.Find(c => c.Id == param.ContentId);
            if (content is null)
                return Task.CompletedTask;
            content.Ratings.First(c => c.AssessorId == param.AssessorId).Value = param.NewValue;
            return Task.CompletedTask;
        }
    }
}
