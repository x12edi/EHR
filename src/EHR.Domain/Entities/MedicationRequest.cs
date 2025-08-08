// EHR.Domain.Entities/MedicationRequest.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class MedicationRequest : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid? EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        public Guid PrescriberId { get; set; }
        public User Prescriber { get; set; }

        public string MedicationCode { get; set; } // RxNorm
        public string MedicationText { get; set; }
        public string Dose { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Duration { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string Status { get; set; } // active/completed/stopped
        public string Reason { get; set; }

        public ICollection<MedicationAdministration> Administrations { get; set; }
    }
}
