using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomTypes.Queries
{
    public record GetRoomTypeByIdQuery(int Id) : IRequest<RoomTypeDto>;

    public class GetRoomTypeByIdQueryHandler : IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>
    {
        private readonly AppDbContext _db;
        public GetRoomTypeByIdQueryHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RoomTypeDto> Handle(GetRoomTypeByIdQuery request, CancellationToken ct)
        {
            var dto = await _db.RoomTypes.AsNoTracking().Where(x => x.Id == request.Id).Select(x => new RoomTypeDto(x.Id, x.Name, x.Description,
                    x.MaxOccupancy, x.BasePrice, x.Amenities)).FirstOrDefaultAsync(ct);
            // dto == null -> ném NotFoundException -> middleware map sang 404
            return dto ?? throw new NotFoundException(nameof(RoomTypes), request.Id);
        }

    }

}
