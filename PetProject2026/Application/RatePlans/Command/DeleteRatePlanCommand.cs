using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RatePlans.Command
{
    public record DeleteRatePlanCommand(int Id) : IRequest;

    public class DeleteRatePlanCommandHandler : IRequestHandler<DeleteRatePlanCommand>
    {
        private readonly AppDbContext _db;
        public DeleteRatePlanCommandHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task Handle(DeleteRatePlanCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.RatePlans.FindAsync(new object?[] { request.Id }, cancellationToken)
                ?? throw new NotFoundException(nameof(RatePlan), request.Id);

            // Không cho xóa rate plan đang được một reservation tham chiếu (sẽ vi phạm FK).
            bool inUse = await _db.Reservations.AnyAsync(r => r.RatePlanId == request.Id, cancellationToken);
            if (inUse)
                throw new ConflictException("Không thể xóa rate plan khi đang có phòng được đặt theo gói giá này.");

            _db.RatePlans.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
