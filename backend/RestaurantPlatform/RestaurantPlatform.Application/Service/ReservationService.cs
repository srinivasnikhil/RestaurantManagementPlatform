using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _uow;

        public ReservationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ReservationDto> CreateAsync(int userId, CreateReservationDto dto)
        {
            var reservation = new Reservation
            {
                UserId = userId,
                ReservationDateTime = dto.ReservationDateTime,
                PartySize = dto.PartySize,
                Notes = dto.Notes,
                Status = ReservationStatus.Pending
            };

            await _uow.Reservations.AddAsync(reservation);
            await _uow.SaveChangesAsync();
            return MapDto(reservation);
        }

        public async Task<IReadOnlyList<ReservationDto>> GetMyReservationsAsync(int userId)
            => (await _uow.Reservations.GetByUserIdAsync(userId)).Select(MapDto).ToList();

        public async Task<IReadOnlyList<ReservationDto>> GetAllAsync()
            => (await _uow.Reservations.GetAllOrderedAsync()).Select(MapDto).ToList();

        public async Task<bool> UpdateStatusAsync(int id, UpdateReservationStatusDto dto)
        {
            var reservation = await _uow.Reservations.GetByIdAsync(id);
            if (reservation is null) return false;

            reservation.Status = dto.Status;
            _uow.Reservations.Update(reservation);
            await _uow.SaveChangesAsync();
            return true;
        }

        private static ReservationDto MapDto(Reservation r) => new()
        {
            Id = r.Id,
            ReservationDateTime = r.ReservationDateTime,
            PartySize = r.PartySize,
            Status = r.Status.ToString(),
            Notes = r.Notes
        };
    }
}
