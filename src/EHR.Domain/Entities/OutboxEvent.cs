// EHR.Domain.Entities/OutboxEvent.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class OutboxEvent
    {
        public long Id { get; set; }
        public string EventType { get; set; }
        public string PayloadJson { get; set; }
        public string Status { get; set; } // Pending/Processing/Sent/Failed
        public int Retries { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? SentAt { get; set; }
    }
}
