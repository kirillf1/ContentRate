using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Rooms;
using ContentRate.Application.Rooms.Decorators;
using ContentRate.Application.Users;
using ContentRate.Domain.Rooms;
using ContentRate.Domain.Users;
using ContentRate.GrpcService.Authorization;
using ContentRate.GrpcService.GrpcServices.Events;
using ContentRate.GrpcService.GrpcServices.Rooms;
using ContentRate.GrpcService.GrpcServices.Users;
using ContentRate.Infrastructure.Contexts;
using ContentRate.Infrastructure.Repositories.EfRepositories;
using ContentRate.Infrastructure.Repositories.EnitiesGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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
builder.Services.AddScoped<IUserContext>(provider =>
{
    var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    if (contextAccessor.HttpContext is null)
        return new UserContextInMemory(null);
    var userClaim = contextAccessor.HttpContext.User;
    if (userClaim is null)
        return new UserContextInMemory(null);
    var nameClaim = userClaim.FindFirst(ClaimTypes.Name);
    var idClaim = userClaim.FindFirst("Id");
    if (nameClaim is null || idClaim is null)
        return new UserContextInMemory(null);
    var id = Guid.Parse(idClaim.Value);
    var name = nameClaim.Value;
    return new UserContextInMemory(new UserTitle() { Id = id, Name = name });
});
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.Decorate<IRoomService, SecureRoomServiceDecorator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RoomEstimateNotifier>();
builder.Services.AddScoped<RoomEstimateEventListenerStorage>();
builder.Services.AddScoped<ITokenGenerator, JwtGenerator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = false,

        };
    });
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapFallbackToFile("index.html");


app.MapGrpcService<RoomEstimateEventServiceGrpc>();
app.MapGrpcService<RoomEstimateServiceGrpc>();
app.MapGrpcService<RoomQueryServiceGrpc>();
app.MapGrpcService<RoomServiceGrpc>();
app.MapGrpcService<AuthServiceGrpc>();
app.MapGrpcService<UserQueryServiceGrpc>();
app.Run();

