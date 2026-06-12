using MediatR;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Enums;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Guests.Command
{
    public record CreateGuestCommand(
    string FullName,
    string Email,
    string Phone,
    IdType IdType,
    string IdNumber,
    string Nationality,
    string Address
) : IRequest<GuestDto>;
    public class CreateguestCommandHandler : IRequestHandler<CreateGuestCommand, GuestDto>
    {
        private readonly AppDbContext _db;
        public CreateguestCommandHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<GuestDto> Handle(CreateGuestCommand request, CancellationToken ct)
        {
            var guest = new Guest
            {
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                IdType = request.IdType,
                IdNumber = request.IdNumber,
                Nationality = request.Nationality,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow
            };
            await _db.Guests.AddAsync(guest, ct);
            await _db.SaveChangesAsync(ct);
            return new GuestDto(
                guest.Id,
                guest.FullName,
                guest.Email,
                guest.Phone,
                guest.IdType.ToString(),
                guest.Nationality,
                guest.DateOfBirth,
                guest.Address,
                guest.Notes,
                guest.CreatedAt
            );
        }
    }
}
