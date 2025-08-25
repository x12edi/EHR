// Application/DTOs/ProviderDtos.cs
namespace EHR.Application.DTOs
{
    public class ProviderDto
    {
        public Guid Id { get; set; }             // ClinicianProfile.Id
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        // Clinician-specific
        public string NPI { get; set; }
        public string LicenseNumber { get; set; }
        public string SpecialtyCode { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string ContactJson { get; set; }
    }

    public class CreateProviderDto
    {
        // User info
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        // Clinician-specific
        public string NPI { get; set; }
        public string LicenseNumber { get; set; }
        public string SpecialtyCode { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? ContactJson { get; set; }
    }

    public class UpdateProviderDto : CreateProviderDto
    {
        public Guid Id { get; set; } // ClinicianProfile.Id
        public Guid UserId { get; set; }
    }
}
