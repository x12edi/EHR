// EHR.Domain.Entities/Allergy.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class Allergy : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public string SubstanceCode { get; set; } // coded
        public string SubstanceText { get; set; } // text name
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string Status { get; set; } // active/inactive
        public DateTimeOffset? RecordedAt { get; set; }
        public string RecordedBy { get; set; }
    }
}
