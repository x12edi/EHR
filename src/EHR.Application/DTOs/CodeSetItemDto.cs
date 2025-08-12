// EHR.Application/DTOs/CodeSetItemDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class CodeSetItemDto
    {
        public Guid Id { get; set; }
        public Guid CodeSetId { get; set; }
        public string ItemCode { get; set; }
        public string Display { get; set; }
        public string System { get; set; }
        public DateTimeOffset? EffectiveFrom { get; set; }
        public DateTimeOffset? EffectiveTo { get; set; }
    }

    public class CreateCodeSetItemDto
    {
        public Guid CodeSetId { get; set; }
        public string ItemCode { get; set; }
        public string Display { get; set; }
        public string System { get; set; }
        public DateTimeOffset? EffectiveFrom { get; set; }
        public DateTimeOffset? EffectiveTo { get; set; }
    }

    public class UpdateCodeSetItemDto : CreateCodeSetItemDto
    {
        public Guid Id { get; set; }
    }
}
