// EHR.Domain.Entities/Problem.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class Problem : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public string Code { get; set; } // ICD/SNOMED
        public string Description { get; set; }
        public DateTimeOffset? OnsetDate { get; set; }
        public DateTimeOffset? ResolvedDate { get; set; }
        public string Status { get; set; } // active/resolved
        public string Severity { get; set; }
    }
}
