using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EHR.IdentityServer.Auth
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _opts;
        private DateTimeOffset _expiry;

        public TokenService(IOptions<JwtOptions> opts)
        {
            _opts = opts.Value;
        }

        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            _expiry = DateTimeOffset.UtcNow.AddMinutes(_opts.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: _expiry.UtcDateTime,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTimeOffset GetExpiry() => _expiry;
    }
}
