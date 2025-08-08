// EHR.Domain.Entities/DiagnosticReport.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class DiagnosticReport : AuditableEntity
    {
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid? OrderId { get; set; }
        public Order Order { get; set; }

        public string ReportType { get; set; }
        public string Summary { get; set; }
        public string ReportJson { get; set; }
        public DateTimeOffset IssuedAt { get; set; }
        public Guid? AuthorId { get; set; }
        public User Author { get; set; }
    }
}
