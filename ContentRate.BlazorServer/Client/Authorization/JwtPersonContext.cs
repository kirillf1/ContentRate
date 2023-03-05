using ContentRate.Application.Contracts.Users;
using ContentRate.Application.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ContentRate.BlazorServer.Client.Authorization
{
    public class JwtPersonContext : IUserContext
    {
        private readonly ITokenProvider tokenProvider;

        public JwtPersonContext(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }
        public async Task<UserTitle?> TryGetCurrentUser()
        {
            var token = await tokenProvider.GetToken();
            if (string.IsNullOrEmpty(token))
                return null;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var name = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            var id = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == "Id").Value);
            return new UserTitle { Id = id, Name = name };
        }
    }
}
