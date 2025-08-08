// EHR.Domain.Entities/AuditAccessLog.cs
using System;

namespace EHR.Domain.Entities
{
    public class AuditAccessLog
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PatientId { get; set; }
        public string Action { get; set; } // View/Download
        public string ResourceType { get; set; }
        public string ResourceId { get; set; }
        public string PurposeOfUse { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public string SourceIp { get; set; }
        public string CorrelationId { get; set; }
    }
}
