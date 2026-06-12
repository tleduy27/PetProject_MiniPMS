using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Guests.Queries
{
    public record GetGuestByIdQuery(int Id) : IRequest<GuestDto>;
    public class GetGuestByIdQueryHandler : IRequestHandler<GetGuestByIdQuery, GuestDto>
    {
        private readonly AppDbContext _appDbContext;
        public GetGuestByIdQueryHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<GuestDto> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
        {
            var guest = await _appDbContext.Guests
            .FirstOrDefaultAsync(
             x => x.Id == request.Id,
             cancellationToken);
            if (guest == null)
            {
                throw new NotFoundException("Guest", request.Id);
            }
            return new GuestDto(
            guest.Id,
            guest.FullName,
            guest.Email,
            guest.Phone,
            guest.IdType.ToString(),
            guest.Nationality,
            guest.DateOfBirth,
            guest.Address,
            guest.Notes,
            guest.CreatedAt
);
        }
    }
}
