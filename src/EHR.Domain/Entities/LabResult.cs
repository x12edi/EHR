// EHR.Domain.Entities/LabResult.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class LabResult : AuditableEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public string TestCode { get; set; } // LOINC
        public string TestName { get; set; }
        public string Value { get; set; }     // keep flexible (string); numeric values can be in separate columns if needed
        public string Unit { get; set; }
        public string ReferenceRange { get; set; }
        public string Flag { get; set; } // H/L/Abnormal
        public DateTimeOffset CollectedAt { get; set; }
        public DateTimeOffset ResultedAt { get; set; }
        public string ReportedBy { get; set; }
        public string RawJson { get; set; } // raw payload
    }
}
