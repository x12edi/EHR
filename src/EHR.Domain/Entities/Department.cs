// EHR.Domain.Entities/Department.cs
using EHR.Domain.Common;
using System.Collections.Generic;

namespace EHR.Domain.Entities
{
    public class Department : AuditableEntity
    {
        public string Name { get; set; }
        public string Type { get; set; } // Clinic/Ward/Lab/Radiology
        public string AddressJson { get; set; }
        public ICollection<ClinicianProfile> Clinicians { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Encounter> Encounters { get; set; }

    }
}
