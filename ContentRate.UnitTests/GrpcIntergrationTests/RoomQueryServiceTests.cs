extern alias GrpcClient;
using ContentRate.Protos;
using ContentRate.UnitTests.GrpcIntergrationTests.Helpers;
using GrpcClient::ContentRate.GrpcClient.Rooms;
using Xunit.Abstractions;

namespace ContentRate.UnitTests.GrpcIntergrationTests
{
    public class RoomQueryServiceTests : IntegrationTestBase
    {
        public RoomQueryServiceTests(GrpcTestFixture<StartupTest> fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {
        }
        [Fact]
        public async Task GetRoomTitles_PreloadedRooms_SuccessRoomsIsNotPrivate()
        {
            const int roomCount = 3;
            var userId = Guid.NewGuid();
            await PreloadUser(userId);
            await PreloadRooms(userId, roomCount, false);
            await PreloadRooms(userId,roomCount, true);
            
            var client = new RoomQueryClientGrpcService(new GrpcClient.ContentRate.Protos.RoomQueryService.RoomQueryServiceClient(Channel));
            var result = await client.GetRoomTitles();

            Assert.True(result.IsSuccess);
            Assert.Contains(result.Value, c =>c.Creator.Id == userId);
        }
        [Fact]
        public async Task GetPersonalRoomTitles_PreloadedRooms_CreatorIdEqualsUser()
        {
            const int roomCount = 3;
            var userId = Guid.NewGuid();
            await PreloadUser(userId);
            
            await PreloadRooms(userId, roomCount, false);
            await PreloadRooms(userId, roomCount, true);

            var client = new RoomQueryClientGrpcService(new GrpcClient.ContentRate.Protos.RoomQueryService.RoomQueryServiceClient(Channel));
            var result = await client.GetPersonalRoomTitles(userId);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Count() == roomCount * 2);
            Assert.True(result.Value.All(c => c.Creator.Id == userId));
        }
       
    }
}
