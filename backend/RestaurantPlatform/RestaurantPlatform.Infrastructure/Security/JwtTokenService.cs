using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantPlatform.Infrastructure.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            // validate required configuration values
            var keyString = jwt["Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("Configuration 'Jwt:Key' is missing or empty.");

            // read raw value and validate it's a positive integer
            var expiryRaw = jwt["ExpireMinutes"];
            if (!int.TryParse(expiryRaw, out var expiryMinutes) || expiryMinutes <= 0)
                throw new InvalidOperationException("Configuration 'Jwt:ExpireMinutes' is missing or not a positive integer.");

            // the secret, turned into a signing key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // the facts we stamp onto the wristband ("claims")
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
