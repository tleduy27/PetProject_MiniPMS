using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Queries
{
    /// <summary>Lấy 1 phòng theo Id, kèm tên loại phòng. Không thấy -> NotFoundException (404).</summary>
    public record GetRoomByIdQuery(int Id) : IRequest<RoomDto>;

    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto>
    {
        private readonly AppDbContext _db;
        public GetRoomByIdQueryHandler(AppDbContext db) => _db = db;

        public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken ct)
            => await _db.Rooms
                   .AsNoTracking()
                   .Where(r => r.Id == request.Id)
                   .Select(r => new RoomDto(
                       r.Id,
                       r.RoomNumber,
                       r.Floor,
                       r.RoomTypeId,
                       r.RoomType.Name,
                       r.Status.ToString(),
                       r.HousekeepingStatus.ToString(),
                       r.Notes))
                   .FirstOrDefaultAsync(ct)
               ?? throw new NotFoundException(nameof(Room), request.Id);
    }
}
