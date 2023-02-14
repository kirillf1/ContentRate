using ContentRate.Application.Rooms;
using ContentRate.BlazorServer.Client.Pages;
using ContentRate.GrpcClient.Rooms;
using ContentRate.ViewModels.Rooms;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RoomQueryService = ContentRate.Protos.RoomQueryService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<MainView>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

//builder.Services.AddGrpcClient<AuthService.AuthServiceClient>(c => new AuthService.AuthServiceClient(channel));
//builder.Services.AddGrpcClient<UserQueryService.UserQueryServiceClient>(c => new UserQueryService.UserQueryServiceClient(channel));
builder.Services.AddGrpcClient<RoomQueryService.RoomQueryServiceClient>(opt=>opt.Address = new("http://localhost:5023"))
    .ConfigureChannel(c=> c.HttpHandler = new GrpcWebHandler(new HttpClientHandler()));

builder.Services.AddScoped<IRoomQueryService, RoomQueryClientGrpcService>();
builder.Services.AddTransient<RoomListViewModel>();
await builder.Build().RunAsync();
