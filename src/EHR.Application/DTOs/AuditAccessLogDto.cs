// DTOs/AuditAccessLogDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class AuditAccessLogDto
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PatientId { get; set; }
        public string Action { get; set; }
        public string ResourceType { get; set; }
        public string ResourceId { get; set; }
        public string PurposeOfUse { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string SourceIp { get; set; }
        public string CorrelationId { get; set; }
    }
}
