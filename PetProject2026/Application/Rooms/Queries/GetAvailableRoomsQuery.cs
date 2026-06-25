using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Enums;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Queries
{
    //Trả về int = số phòng trống
    public record GetAvailableRoomsQuery(int RoomTypeId, DateTime CheckIn, DateTime CheckOut) : IRequest<int>;
    public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, int>
    {
        private readonly AppDbContext _db;
        public GetAvailableRoomsQueryHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<int> Handle(GetAvailableRoomsQuery request, CancellationToken ct)
        {
            // MẢNH 1 — tổng phòng vật lý bán được của loại này
            var totalRooms = await _db.Rooms
                .Where(r => r.RoomTypeId == request.RoomTypeId
                         && r.Status != RoomStatus.OutOfOrder
                         && r.Status != RoomStatus.OutOfService)
                .CountAsync(ct);

            // MẢNH 2 — số booking đang giữ phòng (overlap)
            var booked = await _db.Reservations
                .Where(x => x.RoomTypeId == request.RoomTypeId)
                .Where(x => x.Status == ReservationStatus.Confirmed || x.Status == ReservationStatus.CheckedIn)
                .Where(x => x.CheckInDate < request.CheckOut && x.CheckOutDate > request.CheckIn)
                .CountAsync(ct);

            return totalRooms - booked;   // MẢNH 3
        }
    }
    
}
