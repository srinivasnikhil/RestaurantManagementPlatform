using Microsoft.Extensions.Configuration;
using RestaurantPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RestaurantPlatform.Infrastructure.Payments
{
    public class PayPalService : IPayPalService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        public PayPalService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        private string BaseUrl => _config["PayPal:BaseUrl"] ?? "https://api-m.sandbox.paypal.com";
        private string Currency => _config["PayPal:Currency"] ?? "USD";

        // exchange client id + secret for a short-lived access token
        private async Task<string> GetAccessTokenAsync()
        {
            var clientId = _config["PayPal:ClientId"]!;
            var secret = _config["PayPal:Secret"]!;
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            });

            var res = await _http.SendAsync(request);
            res.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
            return doc.RootElement.GetProperty("access_token").GetString()!;
        }

        public async Task<string> CreateOrderAsync(decimal amount)
        {
            var token = await GetAccessTokenAsync();

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                new { amount = new { currency_code = Currency, value = amount.ToString("0.00") } }
            }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v2/checkout/orders");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(body);

            var res = await _http.SendAsync(request);
            res.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
            return doc.RootElement.GetProperty("id").GetString()!;
        }

        public async Task<PayPalCaptureResult> CaptureOrderAsync(string paypalOrderId)
        {
            var token = await GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v2/checkout/orders/{paypalOrderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");

            var res = await _http.SendAsync(request);
            var json = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
                return new PayPalCaptureResult(false, null, "FAILED");

            using var doc = JsonDocument.Parse(json);
            var status = doc.RootElement.GetProperty("status").GetString() ?? "UNKNOWN";

            string? captureId = null;
            try
            {
                captureId = doc.RootElement
                    .GetProperty("purchase_units")[0]
                    .GetProperty("payments")
                    .GetProperty("captures")[0]
                    .GetProperty("id").GetString();
            }
            catch { /* structure varies; capture id is a nice-to-have */ }

            return new PayPalCaptureResult(status == "COMPLETED", captureId, status);
        }
    }
}
