// EHR.Domain/Common/AuditableEntity.cs
using System;

namespace EHR.Domain.Common
{
    public abstract class AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string CreatedBy { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        // Concurrency token
        public byte[] RowVersion { get; set; }
    }
}
