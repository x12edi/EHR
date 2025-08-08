// EHR.Domain.Entities/Location.cs
using EHR.Domain.Common;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace EHR.Domain.Entities
{
    public class Location : AuditableEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string AddressJson { get; set; }

        public ICollection<Encounter> Encounters { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
