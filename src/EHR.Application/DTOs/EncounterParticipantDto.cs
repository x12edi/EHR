// EHR.Application/DTOs/EncounterParticipantDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class EncounterParticipantDto
    {
        public Guid Id { get; set; }
        public Guid EncounterId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
    }

    public class CreateEncounterParticipantDto
    {
        public Guid EncounterId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
    }

    public class UpdateEncounterParticipantDto : CreateEncounterParticipantDto
    {
        public Guid Id { get; set; }
    }
}
