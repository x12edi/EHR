// EHR.Domain.Entities/Contact.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class Contact : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public string Name { get; set; }
        public string Relationship { get; set; }
        public string PhonesJson { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public bool IsEmergencyContact { get; set; }
    }
}
