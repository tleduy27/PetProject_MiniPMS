using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Queries
{
    /// <summary>Lấy danh sách toàn bộ phòng kèm tên loại phòng.</summary> - Toi la mot request va khi xu li xong toi tra ve list<roomDto></roomDto>
    public record GetRoomsQuery() : IRequest<List<RoomDto>>;
    /// <summary>
    /// GetRoomsQuerhyHandler la handler xu li getRoomQuery va tra ve List<RoomDto></RoomDto>
    /// </summary>
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, List<RoomDto>>
    {
        private readonly AppDbContext _db;

        public GetRoomsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<RoomDto>> Handle(GetRoomsQuery request, CancellationToken ct)
        {
            return await _db.Rooms
                .AsNoTracking()
                .OrderBy(r => r.Floor).ThenBy(r => r.RoomNumber)
                .Select(r => new RoomDto(
                    r.Id,
                    r.RoomNumber,
                    r.Floor,
                    r.RoomTypeId,
                    r.RoomType.Name,
                    r.Status.ToString(),
                    r.HousekeepingStatus.ToString(),
                    r.Notes
                ))
                .ToListAsync(ct);
        }
    }
}
