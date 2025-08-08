// EHR.Domain.Entities/Patient.cs
using EHR.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace EHR.Domain.Entities
{
    public class Patient : AuditableEntity
    {
        public string MRN { get; set; }                 // medical record number, business unique
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullNameNormalized { get; set; }  // computed/normalized for search
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }              // consider enum
        public string PrimaryPhone { get; set; }
        public string Email { get; set; }
        public string PrimaryLanguage { get; set; }
        public string PhotoUrl { get; set; }

        // Flexible details stored as JSON in DB
        public string AddressesJson { get; set; }
        public string IdentifiersJson { get; set; }     // passports, national id, etc
        public string DemographicsJson { get; set; }    // other structured details

        // navigation
        public ICollection<Encounter> Encounters { get; set; }
        public ICollection<Allergy> Allergies { get; set; }
        public ICollection<Problem> Problems { get; set; }
        public ICollection<MedicationRequest> MedicationRequests { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<FileReference> Files { get; set; }

        public ICollection<ClinicalNote> ClinicalNotes { get; set; }
    }
}
