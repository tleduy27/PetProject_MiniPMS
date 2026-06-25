using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomRates.Command
{
    public record DeleteRoomRateCommand(int Id) : IRequest;

  public class DeleteRoomRateCommandHandler : IRequestHandler<DeleteRoomRateCommand>
    {
        private readonly AppDbContext _db;
        public DeleteRoomRateCommandHandler(AppDbContext db) => _db = db;
        public async Task Handle(DeleteRoomRateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _db.RoomRates.FindAsync(new object?[] { request.Id }, cancellationToken)
                ?? throw new NotFoundException(nameof(RoomRate), request.Id);

            _db.RoomRates.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
    }

