// EHR.Domain.Entities/MedicationAdministration.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class MedicationAdministration : AuditableEntity
    {
        public Guid MedicationRequestId { get; set; }
        public MedicationRequest MedicationRequest { get; set; }

        public Guid AdministeredById { get; set; }
        public User AdministeredBy { get; set; }

        public DateTimeOffset AdministeredAt { get; set; }
        public string DoseGiven { get; set; }
        public string Route { get; set; }
        public string Notes { get; set; }
    }
}
