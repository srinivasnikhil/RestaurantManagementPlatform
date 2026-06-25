using RestaurantPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
