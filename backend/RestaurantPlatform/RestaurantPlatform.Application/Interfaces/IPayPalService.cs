using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IPayPalService
    {
        Task<string> CreateOrderAsync(decimal amount);
        Task<PayPalCaptureResult> CaptureOrderAsync(string paypalOrderId);
    }
}
