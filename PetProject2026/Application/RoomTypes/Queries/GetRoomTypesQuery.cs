using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomTypes.Queries
{
    public record GetRoomTypesQuery() : IRequest<List<RoomTypeDto>>;

    public class GetRoomTypesQueryHandler : IRequestHandler<GetRoomTypesQuery, List<RoomTypeDto>>
    {
        private readonly AppDbContext _db;
        public GetRoomTypesQueryHandler(AppDbContext db)
        {
            _db = db;
        }

            public async Task<List<RoomTypeDto>> Handle(GetRoomTypesQuery request, CancellationToken ct)
            => await _db.RoomTypes.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new RoomTypeDto(x.Id, x.Name, x.Description,
                    x.MaxOccupancy, x.BasePrice, x.Amenities))
                .ToListAsync(ct);
    
    }

}
