// EHR.Application/DTOs/ProblemDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class ProblemDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? OnsetDate { get; set; }
        public DateTimeOffset? ResolvedDate { get; set; }
        public string Status { get; set; }
        public string Severity { get; set; }
    }

    public class CreateProblemDto
    {
        public Guid PatientId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? OnsetDate { get; set; }
        public DateTimeOffset? ResolvedDate { get; set; }
        public string Status { get; set; }
        public string Severity { get; set; }
    }

    public class UpdateProblemDto : CreateProblemDto
    {
        public Guid Id { get; set; }
    }
}
