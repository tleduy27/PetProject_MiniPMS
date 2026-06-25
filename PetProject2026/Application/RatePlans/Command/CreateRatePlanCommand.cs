using MediatR;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RatePlans.Command
{
    public record CreateRatePlanCommand
    (
        string Name,
        string? Description,
        bool IsIncludeBreakfast,
        bool IsActive
    ) : IRequest<RatePlanDto>;

    public class CreateRatePlanCommandHandler : IRequestHandler<CreateRatePlanCommand, RatePlanDto>
    {
        private readonly AppDbContext _db;
        public CreateRatePlanCommandHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RatePlanDto> Handle(CreateRatePlanCommand request, CancellationToken cancellationToken)
        {
            var ratePlan = new RatePlan
            {
                Name = request.Name,
                Description = request.Description,
                IsIncludeBreakfast = request.IsIncludeBreakfast,
                IsActive = request.IsActive
            };
            await _db.RatePlans.AddAsync(ratePlan, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return new RatePlanDto(
                ratePlan.Id,
                ratePlan.Name,
                ratePlan.Description,
                ratePlan.IsIncludeBreakfast,
                ratePlan.IsActive
                );
        }
    }
    
}
