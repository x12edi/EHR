// EHR.Domain.Entities/Appointment.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class Appointment : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid? ProviderId { get; set; }
        public User Provider { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }

        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public string Status { get; set; } // scheduled/checked-in/completed/cancelled
        public string? CancelReason { get; set; }
        public DateTimeOffset? CheckInAt { get; set; }
        public DateTimeOffset? CheckOutAt { get; set; }
    }
}
