// EHR.Application/DTOs/MedicationRequestDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class MedicationRequestDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid? EncounterId { get; set; }
        public Guid PrescriberId { get; set; }
        public string MedicationCode { get; set; }
        public string MedicationText { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Duration { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }

    public class CreateMedicationRequestDto
    {
        public Guid PatientId { get; set; }
        public Guid? EncounterId { get; set; }
        public Guid PrescriberId { get; set; }
        public string MedicationCode { get; set; }
        public string MedicationText { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Duration { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }

    public class UpdateMedicationRequestDto : CreateMedicationRequestDto
    {
        public Guid Id { get; set; }
    }
}
