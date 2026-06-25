using MediatR;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomRates.Command
{
    public record UpdateRoomRateCommand
   (int Id, int RoomTypeId, int RatePlanId, DateTime StartDate, DateTime EndDate, decimal Price) : IRequest<RoomRateDto>;
    public class UpdateRoomRateCommandHandler : IRequestHandler<UpdateRoomRateCommand, RoomRateDto>
    {
        private readonly AppDbContext _db;
        public UpdateRoomRateCommandHandler(AppDbContext db) {
            _db = db;
        }
        public async Task<RoomRateDto> Handle(UpdateRoomRateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.RoomRates.FindAsync(new object?[] { request.Id }, cancellationToken)
                ?? throw new NotFoundException(nameof(RoomRate), request.Id);

            entity.RoomTypeId = request.RoomTypeId;
            entity.RatePlanId = request.RatePlanId;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Price = request.Price;

            await _db.SaveChangesAsync(cancellationToken);
            return new RoomRateDto(entity.Id, entity.RoomTypeId, entity.RatePlanId, entity.StartDate, entity.EndDate, entity.Price);
        }
    }
}
