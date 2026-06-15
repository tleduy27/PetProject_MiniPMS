using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Exception;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.RoomTypes.Commands
{
    public record DeleteRoomTypeCommand(int Id) : IRequest;

    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand>
    {
        private readonly AppDbContext _db;
        public DeleteRoomTypeCommandHandler(AppDbContext db) => _db = db;

        public async Task Handle(DeleteRoomTypeCommand request, CancellationToken ct)
        {
            var entity = await _db.RoomTypes.FindAsync(new object?[] { request.Id }, ct)
                ?? throw new NotFoundException(nameof(RoomType), request.Id);

            // RoomType đang có Room/RoomRate tham chiếu (OnDelete Restrict) -> chặn xóa
            bool inUse = await _db.Rooms.AnyAsync(r => r.RoomTypeId == request.Id, ct);
            if (inUse)
                throw new InvalidOperationException("Không thể xóa loại phòng đang có phòng sử dụng.");

            _db.RoomTypes.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }

}
