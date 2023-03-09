using Blazored.LocalStorage;
using ContentRate.Application.Rooms;
using ContentRate.Application.Users;
using ContentRate.BlazorComponents.States;
using ContentRate.BlazorServer.Client.Authorization;
using ContentRate.BlazorServer.Client.Pages;
using ContentRate.GrpcClient.Rooms;
using ContentRate.GrpcClient.Users;
using ContentRate.ViewModels.Rooms;
using ContentRate.ViewModels.Users;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

builder.Services.AddGrpcClient<RoomQueryService.RoomQueryServiceClient>(opt => opt.Address = new(url))
    .ConfigureChannel(c =>
    {
        c.HttpHandler = new GrpcWebHandler(new HttpClientHandler());
        c.UnsafeUseInsecureChannelCallCredentials = true;
    }).AddCallCredentials(async (context, metadata, serviceProvider) => await CallInterceptorMethods.CreateAuthCallCredentials(context, metadata, serviceProvider));
builder.Services.AddGrpcClient<ContentRate.Protos.RoomService.RoomServiceClient>(opt => opt.Address = new(url))
    .ConfigureChannel(c =>
    {
        c.HttpHandler = new GrpcWebHandler(new HttpClientHandler());
        c.UnsafeUseInsecureChannelCallCredentials = true;
    }).AddCallCredentials(async (context, metadata, serviceProvider) => await CallInterceptorMethods.CreateAuthCallCredentials(context, metadata, serviceProvider));
builder.Services.AddGrpcClient<ContentRate.Protos.AuthService.AuthServiceClient>(opt => opt.Address = new(url))
    .ConfigureChannel(c => c.HttpHandler = new GrpcWebHandler(new HttpClientHandler()));

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddScoped<IRoomQueryService, RoomQueryClientGrpcService>();
builder.Services.AddScoped<IRoomService, RoomClientGrpcService>();
builder.Services.AddScoped<IAuthService, AuthClientGrpcService>();
builder.Services.AddTransient<RoomListViewModel>();
builder.Services.AddTransient<RoomEditViewModel>();
builder.Services.AddTransient<RegisterViewModel>();
builder.Services.AddTransient<LoginViewModel>();
builder.Services.AddHttpClient<YoutubeContentImporter>();
builder.Services.AddSingleton<ITokenProvider, LocalStorageTokenProvider>();
builder.Services.AddSingleton<IUserContext, JwtPersonContext>();
builder.Services.AddSingleton<UserAuthState>();

await builder.Build().RunAsync();


internal static class CallInterceptorMethods
{
    //Func<AuthInterceptorContext, Metadata, IServiceProvider, Task>
    public static async Task CreateAuthCallCredentials(AuthInterceptorContext context, Metadata metadata, IServiceProvider serviceProvider)
    {
        var provider = serviceProvider.GetRequiredService<ITokenProvider>();
        var token = await provider.GetToken();
        metadata.Add("Authorization", $"Bearer {token}");
    }
}