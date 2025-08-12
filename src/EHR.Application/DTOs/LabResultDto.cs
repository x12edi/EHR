// EHR.Application/DTOs/LabResultDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class LabResultDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid PatientId { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string Flag { get; set; }
        public DateTimeOffset CollectedAt { get; set; }
        public DateTimeOffset ResultedAt { get; set; }
        public string ReportedBy { get; set; }
        public string RawJson { get; set; }
    }

    public class CreateLabResultDto
    {
        public Guid OrderId { get; set; }
        public Guid PatientId { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string Flag { get; set; }
        public DateTimeOffset CollectedAt { get; set; }
        public DateTimeOffset ResultedAt { get; set; }
        public string ReportedBy { get; set; }
        public string RawJson { get; set; }
    }

    public class UpdateLabResultDto : CreateLabResultDto
    {
        public Guid Id { get; set; }
    }
}
