namespace ContentRate.Application.Users
{
    public interface ITokenProvider
    {
        Task<string> GetToken();
        Task RefreshToken(string token);

    }
}
