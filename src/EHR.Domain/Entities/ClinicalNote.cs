// EHR.Domain.Entities/ClinicalNote.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class ClinicalNote : AuditableEntity
    {
        public Guid? EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public string NoteType { get; set; }   // progress, discharge, consult
        public string Title { get; set; }
        public string Content { get; set; }    // HTML or markdown
        public string StructuredDataJson { get; set; } // vitals, coded fields

        public DateTimeOffset? SignedAt { get; set; }
        public string SignedBy { get; set; }          // user id
        public string SignatureHash { get; set; }     // cryptographic hash
        public bool IsAddendum { get; set; }
        public Guid? ParentNoteId { get; set; }

        public ICollection<FileReference> Files { get; set; }
        
    }
}
