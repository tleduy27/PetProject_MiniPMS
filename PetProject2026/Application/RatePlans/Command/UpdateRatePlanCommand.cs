using MediatR;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RatePlans.Command
{
    public record UpdateRatePlanCommand(int Id, string Name, string? Description, bool IsIncludeBreakfast, bool IsActive) : IRequest<RatePlanDto>;
    
    public class UpdateRatePlanCommandHandler : IRequestHandler<UpdateRatePlanCommand, RatePlanDto>
    {
        private readonly AppDbContext _db;
        public UpdateRatePlanCommandHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RatePlanDto> Handle(UpdateRatePlanCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.RatePlans.FindAsync(new object?[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(RatePlan), request.Id);
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsIncludeBreakfast = request.IsIncludeBreakfast;
            entity.IsActive = request.IsActive;

            await _db.SaveChangesAsync(cancellationToken);
            return new RatePlanDto(entity.Id, entity.Name, entity.Description, entity.IsIncludeBreakfast, entity.IsActive);
        }
    }
}
