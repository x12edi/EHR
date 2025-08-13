namespace EHR.IdentityServer.Auth
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SigningKey { get; set; } = default!; // long random dev key (not for prod)
        public int AccessTokenMinutes { get; set; } = 60;
    }
}
