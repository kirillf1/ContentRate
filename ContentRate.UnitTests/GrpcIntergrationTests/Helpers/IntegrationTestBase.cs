using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ContentRate.UnitTests.GrpcIntergrationTests.Helpers
{
    public class IntegrationTestBase : IClassFixture<GrpcTestFixture<StartupTest>>, IDisposable
    {
        private GrpcChannel? _channel;

        protected GrpcTestFixture<StartupTest> Fixture { get; set; }
        protected GrpcChannel Channel => _channel ??= CreateChannel();
        protected GrpcChannel CreateChannel()
        {
            return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
            {         
                HttpHandler = Fixture.Handler
            });
        }

        public IntegrationTestBase(GrpcTestFixture<StartupTest> fixture, ITestOutputHelper outputHelper)
        {
            Fixture = fixture;
            
        }

        public void Dispose()
        {
            _channel = null;
        }
        protected async Task<User> PreloadUser(Guid id)
        {
            var services = Fixture.ServiceProvider;
            var userRepository = services.GetRequiredService<IUserRepository>();
            var user = UserFactory.CreateUser(id);
            await userRepository.AddUser(user);
            await userRepository.SaveChanges();
            return user;
        }
        protected async Task<IEnumerable<Room>> PreloadRooms(Guid creatorId, int roomCount, bool isPrivate)
        {
            var services = Fixture.ServiceProvider;
            var roomRepository = services.GetRequiredService<IRoomRepository>();
            var roomList = new List<Room>(roomCount);
            for (int i = 0; i < roomCount; i++)
            {
                var room = RoomFactory.CreateRoom(Guid.NewGuid(), 20, creatorId, isPrivate);
                await roomRepository.AddRoom(room);
                roomList.Add(room);
            }
            await roomRepository.SaveChanges();
            return roomList;
        }
    }
}
