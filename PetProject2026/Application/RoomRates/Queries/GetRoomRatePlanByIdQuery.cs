using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomRates.Queries
{
    public record GetRoomRateByIdQuery(int Id) : IRequest<RoomRateDto>;

    public class GetRoomRateByIdQueryHandler : IRequestHandler<GetRoomRateByIdQuery, RoomRateDto>
    {
        private readonly AppDbContext _db;
        public GetRoomRateByIdQueryHandler(AppDbContext db) => _db = db;

        public async Task<RoomRateDto> Handle(GetRoomRateByIdQuery request, CancellationToken ct)
        {
            var dto = await _db.RoomRates.AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new RoomRateDto(x.Id, x.RoomTypeId, x.RatePlanId, x.StartDate, x.EndDate, x.Price))
                .FirstOrDefaultAsync(ct);

            return dto ?? throw new NotFoundException(nameof(RoomRate), request.Id);
        }
    }
}
