using MediatR;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomTypes.Commands
{
    //Data tu client sau khi xong thi tra ve roomTypeDTO
    public record CreateRoomTypeCommand
    (
        string Name,
        string? Description,
        int MaxOccupancy,
        decimal BasePrice,
        string? Amenities
    ) : IRequest<RoomTypeDto>;

    public class  CreateRoomTypeCommandHandler : IRequestHandler<CreateRoomTypeCommand, RoomTypeDto>
    {
        private readonly AppDbContext _db;
        public CreateRoomTypeCommandHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RoomTypeDto> Handle(CreateRoomTypeCommand request, CancellationToken ct)
        {
            var roomType = new Domain.Entities.RoomType
            {
                Name = request.Name,
                Description = request.Description,
                MaxOccupancy = request.MaxOccupancy,
                BasePrice = request.BasePrice,
                Amenities = request.Amenities
            };
            await _db.RoomTypes.AddAsync(roomType, ct);
            await _db.SaveChangesAsync(ct);
            return new RoomTypeDto(
                roomType.Id,
                roomType.Name,
                roomType.Description,
                roomType.MaxOccupancy,
                roomType.BasePrice,
                roomType.Amenities
            );
        }
    }
}
