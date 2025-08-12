// EHR.Application/DTOs/LocationDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Type { get; set; }
        public string? AddressJson { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class CreateLocationDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Type { get; set; }
        public string? AddressJson { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class UpdateLocationDto : CreateLocationDto
    {
        public Guid Id { get; set; }
    }
}
