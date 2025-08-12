// EHR.Application/DTOs/DiagnosticReportDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class DiagnosticReportDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid? OrderId { get; set; }
        public string ReportType { get; set; }
        public string Summary { get; set; }
        public string ReportJson { get; set; }
        public DateTimeOffset IssuedAt { get; set; }
        public Guid? AuthorId { get; set; }
    }

    public class CreateDiagnosticReportDto
    {
        public Guid PatientId { get; set; }
        public Guid? OrderId { get; set; }
        public string ReportType { get; set; }
        public string Summary { get; set; }
        public string ReportJson { get; set; }
        public DateTimeOffset IssuedAt { get; set; }
        public Guid? AuthorId { get; set; }
    }

    public class UpdateDiagnosticReportDto : CreateDiagnosticReportDto
    {
        public Guid Id { get; set; }
    }
}
