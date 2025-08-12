// DTOs/AuditLogDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class AuditLogDto
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string ActionType { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string DetailsJson { get; set; }
        public string SourceIp { get; set; }
        public string CorrelationId { get; set; }
    }
}
