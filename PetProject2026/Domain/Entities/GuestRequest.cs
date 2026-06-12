using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Yêu cầu từ khách (thêm gối, sửa điều hòa...) — theo dõi SLA.</summary>
    public class GuestRequest
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public int RoomId { get; set; }
        public string RequestType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public RequestPriority Priority { get; set; } = RequestPriority.Medium;
        public GuestRequestStatus Status { get; set; } = GuestRequestStatus.Open;
        public string? AssignedTo { get; set; }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }

        // Navigation
        public Reservation? Reservation { get; set; }
        public Room Room { get; set; } = null!;
    }
}
