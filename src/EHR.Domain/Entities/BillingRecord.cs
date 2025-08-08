// EHR.Domain.Entities/BillingRecord.cs
using EHR.Domain.Common;
using System;

namespace EHR.Domain.Entities
{
    public class BillingRecord : AuditableEntity
    {
        public Guid? EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; } // pending/paid/cancelled
        public DateTimeOffset? PaidAt { get; set; }
        public string PaymentReferenceJson { get; set; }
    }
}
