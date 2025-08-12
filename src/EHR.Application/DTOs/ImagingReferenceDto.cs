// EHR.Application/DTOs/ImagingReferenceDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class ImagingReferenceDto
    {
        public Guid Id { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? EncounterId { get; set; }
        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string SOPInstanceUID { get; set; }
        public string Modality { get; set; }
        public string PACSUrl { get; set; }
        public DateTimeOffset? StudyDate { get; set; }
        public string AccessionNumber { get; set; }
    }

    public class CreateImagingReferenceDto
    {
        public Guid? PatientId { get; set; }
        public Guid? EncounterId { get; set; }
        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string SOPInstanceUID { get; set; }
        public string Modality { get; set; }
        public string PACSUrl { get; set; }
        public DateTimeOffset? StudyDate { get; set; }
        public string AccessionNumber { get; set; }
    }

    public class UpdateImagingReferenceDto : CreateImagingReferenceDto
    {
        public Guid Id { get; set; }
    }
}
