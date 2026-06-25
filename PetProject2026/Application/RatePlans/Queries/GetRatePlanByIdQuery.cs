using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Application.RoomTypes.Queries;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RatePlans.Queries
{
    public record GetRatePlanByIdQuery(int id) : IRequest<RatePlanDto>;

    public class GetRatePlanByIdQueryHandler : IRequestHandler<GetRatePlanByIdQuery, RatePlanDto>
    {
        private readonly AppDbContext _db;
        public GetRatePlanByIdQueryHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RatePlanDto> Handle(GetRatePlanByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _db.RatePlans.AsNoTracking().Where(x => x.Id == request.id).Select(x => new RatePlanDto(x.Id, x.Name, x.Description, x.IsIncludeBreakfast, x.IsActive)).FirstOrDefaultAsync(cancellationToken);
            return dto ?? throw new NotFoundException(nameof(RatePlan), request.id);
        }
    }

}
