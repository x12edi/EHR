// EHR.Domain.Entities/Order.cs
using EHR.Domain.Common;
using System;
using System.Collections.Generic;

namespace EHR.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public string OrderNumber { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Guid? EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        public Guid OrderedById { get; set; }
        public User OrderedBy { get; set; }

        public string OrderType { get; set; } // Lab/Imaging/Procedure
        public string OrderStatus { get; set; }
        public DateTimeOffset RequestedAt { get; set; }
        public string Priority { get; set; }
        public string DetailsJson { get; set; }

        public ICollection<LabResult> LabResults { get; set; }
        public ICollection<ImagingReference> ImagingReferences { get; set; }
    }
}
