// EHR.Domain.Entities/Notification.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public Guid? RecipientUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PayloadJson { get; set; }
        public bool IsRead { get; set; }
        public string Severity { get; set; }
        public DateTimeOffset? SentAt { get; set; }
    }
}
