using ContentRate.Application.Rooms;
using ContentRate.Application.Users;
using ContentRate.GrpcService.Authorization;
using ContentRate.GrpcService.GrpcServices.Events;
using ContentRate.GrpcService.GrpcServices.Rooms;
using ContentRate.GrpcService.GrpcServices.Users;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Repositories.EfRepositories;
using ContentRate.UnitTests.GrpcIntergrationTests.Helpers.MockServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContentRate.UnitTests.GrpcIntergrationTests.Helpers
{
    public class StartupTest
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(o => o.EnableDetailedErrors = true);
            services.AddDbContext<ContentRateDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            services.AddSingleton<IRoomRepository, RoomRepositoryEf>();
            services.AddSingleton<IUserRepository, UserRepositoryEf>();
            services.AddSingleton<IContentRepository, ContentRepositoryEf>();
            services.AddSingleton<IRoomService, RoomService>();
            services.AddSingleton<IRoomEstimationService, RoomEstimationService>();
            services.AddSingleton<IRoomQueryService, RoomQueryService>();
            services.AddSingleton<IUserQueryService, UserQueryService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<RoomEstimateNotifier>();
            services.AddSingleton<RoomEstimateEventListenerStorage>();
            services.AddSingleton<ITokenGenerator, TokenMockGenerator>();
            services.AddSingleton<ITokenProvider, TokenMockProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<RoomEstimateEventServiceGrpc>();
                endpoints.MapGrpcService<RoomEstimateServiceGrpc>();
                endpoints.MapGrpcService<RoomQueryServiceGrpc>();
                endpoints.MapGrpcService<RoomServiceGrpc>();
                endpoints.MapGrpcService<AuthServiceGrpc>();
                endpoints.MapGrpcService<UserQueryServiceGrpc>();
            });
        }
    }
}
