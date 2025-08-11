// EHR.Domain.Entities/User.cs
using EHR.Domain.Common;
using System.Collections.Generic;

namespace EHR.Domain.Entities
{
    // Maps to IdentityServer / AspNetUsers row; keep profile extension here
    public class User : AuditableEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsSystemAccount { get; set; }
        public string? Locale { get; set; }
        public string? TimeZone { get; set; }
        public string? ProfileJson { get; set; } // extra profile info

        // navigation
        public ClinicianProfile ClinicianProfile { get; set; }
        public ICollection<ClinicalNote> ClinicalNotesAuthored { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
