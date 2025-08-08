// EHR.Domain.Entities/RefData.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class CodeSet : AuditableEntity
    {
        public string Name { get; set; } // e.g., "ICD-10", "LOINC"
        public string Version { get; set; }
        public string MetadataJson { get; set; }
    }

    public class CodeSetItem : AuditableEntity
    {
        public Guid CodeSetId { get; set; }
        public CodeSet CodeSet { get; set; }
        public string ItemCode { get; set; }
        public string Display { get; set; }
        public string System { get; set; } // system URL for FHIR mapping
        public DateTimeOffset? EffectiveFrom { get; set; }
        public DateTimeOffset? EffectiveTo { get; set; }
    }
}
