// EHR.Domain.Entities/RefreshToken.cs
using System;

namespace EHR.Domain.Entities
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string TokenHash { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? RevokedAt { get; set; }
        public string DeviceInfo { get; set; }
    }
}
