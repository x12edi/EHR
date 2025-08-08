// EHR.Domain.Entities/ImagingReference.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class ImagingReference : AuditableEntity
    {
        public Guid? PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid? EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string SOPInstanceUID { get; set; }
        public string Modality { get; set; }
        public string PACSUrl { get; set; } // URL to retrieve via DICOMweb or proxy
        public DateTimeOffset? StudyDate { get; set; }
        public string AccessionNumber { get; set; }
    }
}
