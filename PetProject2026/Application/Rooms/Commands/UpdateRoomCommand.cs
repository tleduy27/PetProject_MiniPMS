using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Enums;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Commands
{
    public record UpdateRoomCommand(
        int Id, string RoomNumber, int Floor, int roomTypeId, RoomStatus Status, HousekeepingStatus HousekeepingStatus, string? Notes) : IRequest<RoomDto>;
   
    public class UpdateRooomCommandHandler : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        private readonly AppDbContext _db;
        public UpdateRooomCommandHandler(AppDbContext db) => _db = db;
        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken ct)
        {
            //Khong asnotracking o day vi can ef theo doi sinh update
            var entity = await _db.Rooms
                        .Include(r => r.RoomType)
                        .FirstOrDefaultAsync(r => r.Id == request.Id, ct)
                        ?? throw new NotFoundException(nameof(Room), request.Id);
            // Nếu đổi loại phòng -> validate loại mới tồn tại và cập nhật navigation
            // (để DTO trả về đúng tên loại mới, không phải tên cũ đã Include).
            if (entity.RoomTypeId != request.roomTypeId)
            {
                var newType = await _db.RoomTypes
                    .FirstOrDefaultAsync(rt => rt.Id == request.roomTypeId, ct)
                    ?? throw new NotFoundException(nameof(RoomType), request.roomTypeId);
                entity.RoomType = newType;
            }

            entity.RoomNumber = request.RoomNumber;
            entity.Floor = request.Floor;
            entity.RoomTypeId = request.roomTypeId;
            entity.Status = request.Status;
            entity.HousekeepingStatus = request.HousekeepingStatus;
            entity.Notes = request.Notes;
            await _db.SaveChangesAsync(ct);
            return new RoomDto(entity.Id, entity.RoomNumber, entity.Floor,
                entity.RoomTypeId, entity.RoomType.Name, entity.Status.ToString(),
                entity.HousekeepingStatus.ToString(), entity.Notes);
        }
    }
}
