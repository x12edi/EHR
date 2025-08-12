// EHR.Application/DTOs/AllergyDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class AllergyDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string SubstanceCode { get; set; }
        public string SubstanceText { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? RecordedAt { get; set; }
        public string RecordedBy { get; set; }
    }

    public class CreateAllergyDto
    {
        public Guid PatientId { get; set; }
        public string SubstanceCode { get; set; }
        public string SubstanceText { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? RecordedAt { get; set; }
        public string RecordedBy { get; set; }
    }

    public class UpdateAllergyDto : CreateAllergyDto
    {
        public Guid Id { get; set; }
    }
}
