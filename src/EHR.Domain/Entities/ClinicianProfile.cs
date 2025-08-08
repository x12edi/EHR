// EHR.Domain.Entities/ClinicianProfile.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class ClinicianProfile : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string NPI { get; set; }
        public string LicenseNumber { get; set; }
        public string SpecialtyCode { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }
        public string ContactJson { get; set; }
    }
}
