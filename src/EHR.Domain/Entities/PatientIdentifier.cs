// EHR.Domain.Entities/PatientIdentifier.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class PatientIdentifier : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public string IdentifierType { get; set; }  // e.g., "Passport", "Aadhar"
        public string Value { get; set; }
        public string Issuer { get; set; }
        public DateTimeOffset? AssignedAt { get; set; }
    }
}
