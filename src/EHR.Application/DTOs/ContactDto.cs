// EHR.Application/DTOs/ContactDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string PhonesJson { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public bool IsEmergencyContact { get; set; }
    }

    public class CreateContactDto
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string PhonesJson { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public bool IsEmergencyContact { get; set; }
    }

    public class UpdateContactDto : CreateContactDto
    {
        public Guid Id { get; set; }
    }
}
