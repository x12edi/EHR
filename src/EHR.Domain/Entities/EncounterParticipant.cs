// EHR.Domain.Entities/EncounterParticipant.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class EncounterParticipant : AuditableEntity
    {
        public Guid EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Role { get; set; } // attending/nurse/observer
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
    }
}
