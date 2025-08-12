// EHR.Application/DTOs/NotificationDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid? RecipientUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PayloadJson { get; set; }
        public bool IsRead { get; set; }
        public string Severity { get; set; }
        public DateTimeOffset? SentAt { get; set; }
    }

    public class CreateNotificationDto
    {
        public Guid? RecipientUserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PayloadJson { get; set; }
        public bool IsRead { get; set; }
        public string Severity { get; set; }
        public DateTimeOffset? SentAt { get; set; }
    }

    public class UpdateNotificationDto : CreateNotificationDto
    {
        public Guid Id { get; set; }
    }
}
