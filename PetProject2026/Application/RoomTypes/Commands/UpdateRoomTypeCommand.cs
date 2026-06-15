using MediatR;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomTypes.Commands
{
    public record UpdateRoomTypeCommand(
        int Id, string Name, string? Description,
        int MaxOccupancy, decimal BasePrice, string? Amenities
    ) : IRequest<RoomTypeDto>;

    public class UpdateRoomTypeCommandHandler : IRequestHandler<UpdateRoomTypeCommand, RoomTypeDto>
    {
        private readonly AppDbContext _db;
        public UpdateRoomTypeCommandHandler(AppDbContext db) => _db = db;

        public async Task<RoomTypeDto> Handle(UpdateRoomTypeCommand request, CancellationToken ct)
        {
            // KHÔNG AsNoTracking ở đây — cần EF theo dõi để sinh UPDATE
            var entity = await _db.RoomTypes.FindAsync(new object?[] { request.Id }, ct)
                ?? throw new NotFoundException(nameof(RoomType), request.Id);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.MaxOccupancy = request.MaxOccupancy;
            entity.BasePrice = request.BasePrice;
            entity.Amenities = request.Amenities;

            await _db.SaveChangesAsync(ct); // EF tự phát hiện thay đổi -> UPDATE

            return new RoomTypeDto(entity.Id, entity.Name, entity.Description,
                entity.MaxOccupancy, entity.BasePrice, entity.Amenities);
        }
    }
}
