// EHR.Application/DTOs/MedicationAdministrationDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class MedicationAdministrationDto
    {
        public Guid Id { get; set; }
        public Guid MedicationRequestId { get; set; }
        public Guid AdministeredById { get; set; }
        public DateTimeOffset AdministeredAt { get; set; }
        public string DoseGiven { get; set; }
        public string Route { get; set; }
        public string Notes { get; set; }
    }

    public class CreateMedicationAdministrationDto
    {
        public Guid MedicationRequestId { get; set; }
        public Guid AdministeredById { get; set; }
        public DateTimeOffset AdministeredAt { get; set; }
        public string DoseGiven { get; set; }
        public string Route { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateMedicationAdministrationDto : CreateMedicationAdministrationDto
    {
        public Guid Id { get; set; }
    }
}
