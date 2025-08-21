namespace EHR.Application.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid? ProviderId { get; set; }
        public string ProviderName { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public string Status { get; set; }
        public string? CancelReason { get; set; }
        public DateTimeOffset? CheckInAt { get; set; }
        public DateTimeOffset? CheckOutAt { get; set; }
    }

    public class CreateAppointmentDto
    {
        public Guid PatientId { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public string Status { get; set; }
        public string? CancelReason { get; set; }
    }

    public class UpdateAppointmentDto
    {
        public Guid PatientId { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public DateTimeOffset EndAt { get; set; }
        public string Status { get; set; }
        public string? CancelReason { get; set; }
        public DateTimeOffset? CheckInAt { get; set; }
        public DateTimeOffset? CheckOutAt { get; set; }
    }
}
