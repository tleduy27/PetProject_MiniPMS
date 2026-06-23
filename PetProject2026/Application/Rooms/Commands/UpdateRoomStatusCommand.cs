using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Enums;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Commands
{
    public record UpdateRoomStatusCommand(int Id, RoomStatus NewStatus) : IRequest<RoomDto>;

    public class UpdateRoomStatusCommandHandler : IRequestHandler<UpdateRoomStatusCommand, RoomDto>
    {
        private readonly AppDbContext _db;
        public UpdateRoomStatusCommandHandler(AppDbContext db) => _db = db;

        public async Task<RoomDto> Handle(UpdateRoomStatusCommand request, CancellationToken ct)
        {
            // 1. Tồn tại — Include RoomType để lấy Name cho DTO (FindAsync không load navigation)
            var room = await _db.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == request.Id, ct)
                ?? throw new NotFoundException(nameof(Room), request.Id);

            // 2. Idempotent — không đổi thì trả về luôn, khỏi ghi DB
            if (room.Status == request.NewStatus)
                return ToDto(room);

            // 3. Hợp lệ? — hỏi chính entity
            if (!room.CanChangeTo(request.NewStatus))
                throw new ConflictException(
                    $"Không thể chuyển trạng thái phòng từ {room.Status} sang {request.NewStatus}.");

            // 4. Cập nhật
            room.Status = request.NewStatus;
            await _db.SaveChangesAsync(ct);

            return ToDto(room);
        }

        private static RoomDto ToDto(Room r) => new(
            r.Id, r.RoomNumber, r.Floor, r.RoomTypeId, r.RoomType.Name,
            r.Status.ToString(), r.HousekeepingStatus.ToString(), r.Notes);
    }
}
