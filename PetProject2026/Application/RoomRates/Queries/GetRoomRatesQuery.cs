using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomRates.Queries
{
    /// <summary>Danh sách giá. Lọc tùy chọn theo RoomTypeId.</summary>
    public record GetRoomRatesQuery(int? RoomTypeId = null) : IRequest<List<RoomRateDto>>;

    public class GetRoomRatesQueryHandler : IRequestHandler<GetRoomRatesQuery, List<RoomRateDto>>
    {
        private readonly AppDbContext _db;
        public GetRoomRatesQueryHandler(AppDbContext db) => _db = db;

        public async Task<List<RoomRateDto>> Handle(GetRoomRatesQuery request, CancellationToken ct)
        {
            var query = _db.RoomRates.AsNoTracking();

            if (request.RoomTypeId is not null)
                query = query.Where(x => x.RoomTypeId == request.RoomTypeId);

            return await query
                .OrderBy(x => x.RoomTypeId).ThenBy(x => x.StartDate)
                .Select(x => new RoomRateDto(x.Id, x.RoomTypeId, x.RatePlanId, x.StartDate, x.EndDate, x.Price))
                .ToListAsync(ct);
        }
    }
}
