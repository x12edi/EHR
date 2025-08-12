// EHR.Application/DTOs/BillingRecordDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class BillingRecordDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid EncounterId { get; set; }
        public DateTimeOffset BillingDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }

    public class CreateBillingRecordDto
    {
        public Guid PatientId { get; set; }
        public Guid EncounterId { get; set; }
        public DateTimeOffset BillingDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
    }

    public class UpdateBillingRecordDto : CreateBillingRecordDto
    {
        public Guid Id { get; set; }
    }
}
