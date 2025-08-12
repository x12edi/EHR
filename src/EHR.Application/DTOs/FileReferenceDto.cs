// EHR.Application/DTOs/FileReferenceDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class FileReferenceDto
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long SizeBytes { get; set; }
        public string StoragePath { get; set; }
        public string Checksum { get; set; }
        public bool IsPrivate { get; set; }
        public string MetadataJson { get; set; }
    }

    public class CreateFileReferenceDto
    {
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long SizeBytes { get; set; }
        public string StoragePath { get; set; }
        public string Checksum { get; set; }
        public bool IsPrivate { get; set; }
        public string MetadataJson { get; set; }
    }

    public class UpdateFileReferenceDto : CreateFileReferenceDto
    {
        public Guid Id { get; set; }
    }
}
