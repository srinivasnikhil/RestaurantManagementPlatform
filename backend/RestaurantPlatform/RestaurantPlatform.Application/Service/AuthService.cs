using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using RestaurantPlatform.Domain.Entities;
using RestaurantPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(IUnitOfWork uow, ITokenService tokenService, IPasswordHasher passwordHasher) 
        { 
            _uow = uow;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterDTO dto)
        {
            // rule: one account per email
            var existing = await _uow.Users.GetByEmailAsync(dto.Email);
            if (existing is not null) return null;

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),   // store the fingerprint, never the password
                Role = UserRole.Customer
            };

            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();

            return BuildResponse(user);
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var user = await _uow.Users.GetByEmailAsync(dto.Email);
            if (user is null) return null;

            var valid = _passwordHasher.Verify(dto.Password, user.PasswordHash);
            if (!valid) return null;

            return BuildResponse(user);
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            return user is null ? null : MapUser(user);
        }

        private AuthResponseDTO BuildResponse(User user) => new()
        {
            Token = _tokenService.CreateToken(user),
            User = MapUser(user)
        };

        private static UserDTO MapUser(User u) => new()
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role.ToString()
        };
    }
}
