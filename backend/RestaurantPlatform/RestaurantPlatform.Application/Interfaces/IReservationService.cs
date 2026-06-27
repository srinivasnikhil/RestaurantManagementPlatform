using RestaurantPlatform.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateAsync(int userId, CreateReservationDto dto);
        Task<IReadOnlyList<ReservationDto>> GetMyReservationsAsync(int userId);
        Task<IReadOnlyList<ReservationDto>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int id, UpdateReservationStatusDto dto);
    }
}
