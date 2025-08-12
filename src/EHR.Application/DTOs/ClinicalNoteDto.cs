// EHR.Application/DTOs/ClinicalNoteDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class ClinicalNoteDto
    {
        public Guid Id { get; set; }
        public Guid? PatientId { get; set; }
        public Guid AuthorId { get; set; }
        public string NoteType { get; set; }
        public string Content { get; set; }
        public string StructuredDataJson { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateClinicalNoteDto
    {
        public Guid? PatientId { get; set; }
        public Guid AuthorId { get; set; }
        public string NoteType { get; set; }
        public string Content { get; set; }
        public string StructuredDataJson { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class UpdateClinicalNoteDto : CreateClinicalNoteDto
    {
        public Guid Id { get; set; }
    }
}
