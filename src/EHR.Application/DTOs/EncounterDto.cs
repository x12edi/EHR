// EHR.Application/DTOs/EncounterDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class EncounterDto
    {
        public Guid Id { get; set; }
        public string EncounterNumber { get; set; }
        public Guid PatientId { get; set; }
        public string EncounterType { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public string Status { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? PrimaryProviderId { get; set; }
        public string ChiefComplaint { get; set; }
        public string VisitReasonCode { get; set; }
        public string NotesSummary { get; set; }
    }

    public class CreateEncounterDto
    {
        public string EncounterNumber { get; set; }
        public Guid PatientId { get; set; }
        public string EncounterType { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public string Status { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? PrimaryProviderId { get; set; }
        public string ChiefComplaint { get; set; }
        public string VisitReasonCode { get; set; }
        public string NotesSummary { get; set; }
    }

    public class UpdateEncounterDto : CreateEncounterDto
    {
        public Guid Id { get; set; }
    }
}
