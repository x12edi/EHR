// EHR.Application/DTOs/ScheduleSlotDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class ScheduleSlotDto
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? LocationId { get; set; }
        public string RecurrenceJson { get; set; }
    }

    public class CreateScheduleSlotDto
    {
        public Guid ProviderId { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? LocationId { get; set; }
        public string RecurrenceJson { get; set; }
    }

    public class UpdateScheduleSlotDto : CreateScheduleSlotDto
    {
        public Guid Id { get; set; }
    }
}
