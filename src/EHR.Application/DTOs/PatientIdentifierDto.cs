// EHR.Application/DTOs/PatientIdentifierDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class PatientIdentifierDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string IdentifierType { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
        public DateTimeOffset? AssignedAt { get; set; }
    }

    public class CreatePatientIdentifierDto
    {
        public Guid PatientId { get; set; }
        public string IdentifierType { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
        public DateTimeOffset? AssignedAt { get; set; }
    }

    public class UpdatePatientIdentifierDto : CreatePatientIdentifierDto
    {
        public Guid Id { get; set; }
    }
}
