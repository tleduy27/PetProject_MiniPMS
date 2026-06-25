using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomRates.Command
{
    public record CreateRoomRateCommand
    (
        int RoomTypeId,
        int RatePlanId,
        DateTime StartDate,
        DateTime EndDate,
        decimal Price
    ) : IRequest<RoomRateDto>;

    public class CreateRoomRateCommandHandler : IRequestHandler<CreateRoomRateCommand, RoomRateDto>
    {
        private readonly AppDbContext _db;
        public CreateRoomRateCommandHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RoomRateDto> Handle(CreateRoomRateCommand request, CancellationToken cancellationToken)
        {
            // Chống trùng giá: cùng RoomType + RatePlan mà khoảng ngày ĐÈ nhau (khoảng đóng).
            // Hai đoạn [s1,e1] và [s2,e2] đè nhau khi:  s1 <= e2  &&  e1 >= s2
            bool isOverlapping = await _db.RoomRates
                .Where(r => r.RoomTypeId == request.RoomTypeId && r.RatePlanId == request.RatePlanId)
                .AnyAsync(r => r.StartDate <= request.EndDate && r.EndDate >= request.StartDate, cancellationToken);
            if (isOverlapping)
                throw new ConflictException("Khoảng ngày này đã có giá cho loại phòng + gói giá đã chọn.");

            var roomRate = new RoomRate
            {
                RoomTypeId = request.RoomTypeId,
                RatePlanId = request.RatePlanId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Price = request.Price
            };
            await _db.RoomRates.AddAsync( roomRate,cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return new RoomRateDto(
                roomRate.Id,
                roomRate.RoomTypeId,
                roomRate.RatePlanId,
                roomRate.StartDate,
                roomRate.EndDate,
                roomRate.Price
                );

        }
    }
}
