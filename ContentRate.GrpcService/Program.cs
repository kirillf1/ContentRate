using ContentRate.Application.Rooms;
using ContentRate.Application.Users;
using ContentRate.Domain.Rooms;
using ContentRate.Domain.Users;
using ContentRate.GrpcService.GrpcServices.Events;
using ContentRate.GrpcService.GrpcServices.Rooms;
using ContentRate.GrpcService.GrpcServices.Users;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Repositories.EfRepositories;
using ContentRate.Infrastructure.Repositories.EnitiesGenerator;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<ContentRateDbContext>(c =>
c.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<IRoomRepository, RoomGenerator>();
builder.Services.AddScoped<IUserRepository, UserRepositoryEf>();
builder.Services.AddScoped<IContentRepository, ContentRepositoryEf>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomEstimationService, RoomEstimationService>();
builder.Services.AddScoped<IRoomQueryService, RoomQueryService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RoomEstimateNotifier>();
builder.Services.AddScoped<RoomEstimateEventListenerStorage>();

var app = builder.Build();
using var services = app.Services.CreateScope();
using var db = services.ServiceProvider.GetRequiredService<ContentRateDbContext>();
await db.Database.MigrateAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();


app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.MapFallbackToFile("index.html");

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.MapGrpcService<RoomEstimateEventServiceGrpc>();
app.MapGrpcService<RoomEstimateServiceGrpc>();
app.MapGrpcService<RoomQueryServiceGrpc>();
app.MapGrpcService<RoomServiceGrpc>();
app.MapGrpcService<AuthServiceGrpc>();
app.MapGrpcService<UserQueryServiceGrpc>();
app.Run();
