// EHR.Domain.Entities/AuditLog.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class AuditLog
    {
        public long Id { get; set; } // bigint identity
        public string UserId { get; set; }
        public string ActionType { get; set; }  // create/read/update/delete
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public string DetailsJson { get; set; } // { before:..., after:... }
        public string SourceIp { get; set; }
        public string CorrelationId { get; set; }
    }
}
