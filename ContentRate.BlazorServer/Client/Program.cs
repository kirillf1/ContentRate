using ContentRate.Application.Rooms;
using ContentRate.Application.Users;
using ContentRate.BlazorServer.Client.Pages;
using ContentRate.GrpcClient.Rooms;
using ContentRate.GrpcClient.Users;
using ContentRate.ViewModels.Rooms;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Youtube.Extensions;
using RoomQueryService = ContentRate.Protos.RoomQueryService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<MainView>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

//builder.Services.AddGrpcClient<AuthService.AuthServiceClient>(c => new AuthService.AuthServiceClient(channel));
//builder.Services.AddGrpcClient<UserQueryService.UserQueryServiceClient>(c => new UserQueryService.UserQueryServiceClient(channel));
var url = builder.Configuration["ServerUrl"];
builder.Services.AddGrpcClient<RoomQueryService.RoomQueryServiceClient>(opt=>opt.Address = new(url))
    .ConfigureChannel(c=> c.HttpHandler = new GrpcWebHandler(new HttpClientHandler()));
builder.Services.AddGrpcClient<ContentRate.Protos.RoomService.RoomServiceClient>(opt => opt.Address = new(url))
    .ConfigureChannel(c => c.HttpHandler = new GrpcWebHandler(new HttpClientHandler()));
builder.Services.AddGrpcClient<ContentRate.Protos.AuthService.AuthServiceClient>(opt => opt.Address = new(url))
    .ConfigureChannel(c => c.HttpHandler = new GrpcWebHandler(new HttpClientHandler()));

builder.Services.AddScoped<IRoomQueryService, RoomQueryClientGrpcService>();
builder.Services.AddScoped<IRoomService, RoomClientGrpcService>();
builder.Services.AddScoped<IAuthService, AuthClientGrpcService>();
builder.Services.AddTransient<RoomListViewModel>();
builder.Services.AddTransient<RoomEditViewModel>();
builder.Services.AddHttpClient<YoutubeContentImporter>();
await builder.Build().RunAsync();
