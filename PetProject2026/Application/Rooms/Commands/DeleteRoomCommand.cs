using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Enums;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Commands
{
    public record DeleteRoomCommand(int Id) : IRequest;

    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
    {
        private readonly AppDbContext _db;
        public DeleteRoomCommandHandler(AppDbContext db) => _db = db;
        public async Task Handle(DeleteRoomCommand request, CancellationToken ct)
        {
            var entity = await _db.Rooms.FindAsync(new object?[] { request.Id }, ct)
                ?? throw new NotFoundException(nameof(Room), request.Id);

            // Chặn xóa khi phòng còn booking đang hoạt động (Confirmed/CheckedIn) trỏ tới.
            // Xóa cứng lúc này sẽ làm mồ côi reservation -> vi phạm toàn vẹn dữ liệu.
            var hasActiveReservation = await _db.Reservations.AnyAsync(r =>
                r.RoomId == request.Id &&
                (r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.CheckedIn),
                ct);
            if (hasActiveReservation)
                throw new ConflictException("Phòng đang có booking hoạt động, không thể xóa.");

            _db.Rooms.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }

}
