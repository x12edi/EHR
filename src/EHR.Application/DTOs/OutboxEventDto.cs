// EHR.Application/DTOs/OutboxEventDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class OutboxEventDto
    {
        public long Id { get; set; }
        public string EventType { get; set; }
        public string PayloadJson { get; set; }
        public string Status { get; set; }
        public int Retries { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? SentAt { get; set; }
    }

    public class CreateOutboxEventDto
    {
        public string EventType { get; set; }
        public string PayloadJson { get; set; }
        public string Status { get; set; }
        public int Retries { get; set; }
        public DateTimeOffset? SentAt { get; set; }
    }

    public class UpdateOutboxEventDto : CreateOutboxEventDto
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
