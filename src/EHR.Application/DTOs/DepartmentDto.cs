// EHR.Application/DTOs/DepartmentDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? AddressJson { get; set; }
    }

    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? AddressJson { get; set; }
    }

    public class UpdateDepartmentDto : CreateDepartmentDto
    {
        public Guid Id { get; set; }
    }
}
