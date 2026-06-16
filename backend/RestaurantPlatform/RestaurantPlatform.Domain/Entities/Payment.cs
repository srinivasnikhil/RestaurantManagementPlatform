using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string? StripePaymentIntentId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public decimal Amount { get; set; }
    }
}
