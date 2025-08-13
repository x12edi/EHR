using System.Security.Claims;

namespace EHR.IdentityServer.Auth
{
    public interface ITokenService
    {
        string CreateAccessToken(IEnumerable<Claim> claims);
        DateTimeOffset GetExpiry();
    }
}
