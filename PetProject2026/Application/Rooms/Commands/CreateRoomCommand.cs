using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Enums;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Commands
{
    public record CreateRoomCommand(
        string RoomNumber,
        int Floor,
        int RoomTypeId,
        RoomStatus Status,
        HousekeepingStatus HousekeepingStatus,
        string? Notes
        ) : IRequest<RoomDto>;

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        private readonly AppDbContext appDbContext;
        public CreateRoomCommandHandler(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            // Validate RoomType tồn tại (khóa ngoại) — lấy luôn Name để trả về DTO.
            // KHÔNG new RoomType: đã có RoomTypeId, new sẽ tạo loại phòng trùng trong DB.
            var roomType = await appDbContext.RoomTypes
                .FirstOrDefaultAsync(rt => rt.Id == request.RoomTypeId, cancellationToken)
                ?? throw new NotFoundException(nameof(RoomType), request.RoomTypeId);

            var room = new Room
            {
                RoomNumber = request.RoomNumber,
                Floor = request.Floor,
                RoomTypeId = request.RoomTypeId,
                Status = request.Status,
                HousekeepingStatus = request.HousekeepingStatus,
                Notes = request.Notes
            };
            appDbContext.Rooms.Add(room);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return new RoomDto(
                room.Id,
                room.RoomNumber,
                room.Floor,
                room.RoomTypeId,
                roomType.Name,
                room.Status.ToString(),
                room.HousekeepingStatus.ToString(),
                room.Notes
            );
        }
    }
}
