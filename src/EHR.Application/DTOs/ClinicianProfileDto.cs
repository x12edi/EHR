// EHR.Application/DTOs/ClinicianProfileDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class ClinicianProfileDto
    {
        public Guid Id { get; set; }
        public string LicenseNumber { get; set; }
        public string Npi { get; set; }
        public string SpecialtyCode { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateClinicianProfileDto
    {
        public string LicenseNumber { get; set; }
        public string Npi { get; set; }
        public string SpecialtyCode { get; set; }
        public Guid UserId { get; set; }
    }

    public class UpdateClinicianProfileDto : CreateClinicianProfileDto
    {
        public Guid Id { get; set; }
    }
}
