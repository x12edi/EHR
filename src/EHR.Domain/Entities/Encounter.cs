// EHR.Domain.Entities/Encounter.cs
using EHR.Domain.Common;
using System;
using System.Collections.Generic;

namespace EHR.Domain.Entities
{
    public class Encounter : AuditableEntity
    {
        public string EncounterNumber { get; set; } // business id
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public string EncounterType { get; set; } // inpatient/outpatient/tele/emergency
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public string Status { get; set; } // planned/inprogress/finished/cancelled
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid? PrimaryProviderId { get; set; }
        public User PrimaryProvider { get; set; }

        public string ChiefComplaint { get; set; }
        public string VisitReasonCode { get; set; } // ICD/SNOMED
        public string NotesSummary { get; set; }

        public ICollection<EncounterParticipant> Participants { get; set; }
        public ICollection<ClinicalNote> ClinicalNotes { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ImagingReference> ImagingReferences { get; set; }
        public ICollection<FileReference> Files { get; set; }
    }
}
