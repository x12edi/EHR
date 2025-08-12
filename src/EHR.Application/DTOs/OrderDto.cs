// EHR.Application/DTOs/OrderDto.cs
using System;

namespace EHR.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid PatientId { get; set; }
        public Guid? EncounterId { get; set; }
        public Guid OrderedById { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public DateTimeOffset RequestedAt { get; set; }
        public string Priority { get; set; }
        public string DetailsJson { get; set; }
    }

    public class CreateOrderDto
    {
        public string OrderNumber { get; set; }
        public Guid PatientId { get; set; }
        public Guid? EncounterId { get; set; }
        public Guid OrderedById { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public DateTimeOffset RequestedAt { get; set; }
        public string Priority { get; set; }
        public string DetailsJson { get; set; }
    }

    public class UpdateOrderDto : CreateOrderDto
    {
        public Guid Id { get; set; }
    }
}
