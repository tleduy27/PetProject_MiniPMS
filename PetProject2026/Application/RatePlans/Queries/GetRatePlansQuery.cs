using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RatePlans.Queries
{
    public record GetRatePlansQuery() : IRequest<List<RatePlanDto>>;
    public class GetRatePlansQueryHandler : IRequestHandler<GetRatePlansQuery, List<RatePlanDto>>
    {
        private readonly AppDbContext _db;
        public GetRatePlansQueryHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<RatePlanDto>> Handle(GetRatePlansQuery request, CancellationToken cancellationToken)
        {
            var ratePlan = await _db.RatePlans.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new RatePlanDto(x.Id,x.Name, x.Description, x.IsIncludeBreakfast, x.IsActive)).ToListAsync(cancellationToken);
            return ratePlan;
        }

    }
    }
