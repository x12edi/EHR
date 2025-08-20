// EHR.Application/DTOs/PatientDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string MRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullNameNormalized { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string PrimaryPhone { get; set; }
        public string Email { get; set; }
        public string PrimaryLanguage { get; set; }
        public string PhotoUrl { get; set; }
        public string AddressesJson { get; set; }
        public string IdentifiersJson { get; set; }
        public string DemographicsJson { get; set; }
    }

    public class CreatePatientDto
    {
        public string MRN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? FullNameNormalized { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string PrimaryPhone { get; set; }
        public string Email { get; set; }
        public string? PrimaryLanguage { get; set; }
        public string? PhotoUrl { get; set; }
        public string? AddressesJson { get; set; }
        public string? IdentifiersJson { get; set; }
        public string? DemographicsJson { get; set; }
    }

    public class UpdatePatientDto : CreatePatientDto
    {
        public Guid Id { get; set; }
    }
}
