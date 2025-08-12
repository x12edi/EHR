// EHR.Application/DTOs/UserDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string SubjectId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsSystemAccount { get; set; }
        public string? Locale { get; set; }
        public string? TimeZone { get; set; }
        public string? ProfileJson { get; set; }
    }

    public class CreateUserDto
    {
        public string SubjectId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsSystemAccount { get; set; }
        public string? Locale { get; set; }
        public string? TimeZone { get; set; }
        public string? ProfileJson { get; set; }
    }

    public class UpdateUserDto : CreateUserDto
    {
        public Guid Id { get; set; }
    }
}
