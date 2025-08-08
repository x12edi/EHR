// EHR.Domain.Entities/ScheduleSlot.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class ScheduleSlot : AuditableEntity
    {
        public Guid ProviderId { get; set; }
        public User Provider { get; set; }

        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }
        public string RecurrenceJson { get; set; } // optional recurrence rule
    }
}
