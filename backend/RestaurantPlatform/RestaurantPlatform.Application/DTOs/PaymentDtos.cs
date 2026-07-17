using System;
using System.Collections.Generic;
using System.Text;

public record PayPalCaptureResult(bool Success, string? CaptureId, string Status);

namespace RestaurantPlatform.Application.DTOs
{
    public class CapturePaymentDto
    {
        public string PaypalOrderId { get; set; } = string.Empty;
        public PlaceGuestOrderDto Order { get; set; } = new();
    }
}
