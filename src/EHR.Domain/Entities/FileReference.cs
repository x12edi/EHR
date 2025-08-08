// EHR.Domain.Entities/FileReference.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class FileReference : AuditableEntity
    {
        public string EntityType { get; set; }        // e.g., "Patient","Encounter","ClinicalNote"
        public Guid? EntityId { get; set; }           // polymorphic
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long SizeBytes { get; set; }
        public string StoragePath { get; set; }       // local path or object storage key
        public string Checksum { get; set; }
        public bool IsPrivate { get; set; } = true;
        public string MetadataJson { get; set; }      // e.g., DICOM tags minimal
    }
}
