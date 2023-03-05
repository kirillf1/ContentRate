using ContentRate.Application.Contracts.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ContentRate.GrpcService.Authorization
{
    public class JwtGenerator : ITokenGenerator
    {
        public Task<string> GenerateToken(UserTitle userTitle)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, userTitle.Name),
                new Claim("Id", userTitle.Id.ToString())
            };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}
