using System.ComponentModel.DataAnnotations;
using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Đặt phòng.</summary>
    public class Reservation
    {
        public int Id { get; set; }
        public string ReservationNumber { get; set; } = string.Empty;

        public int GuestId { get; set; }
        public int RoomTypeId { get; set; }
        public int? RoomId { get; set; }            // gán khi check-in

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? ActualCheckIn { get; set; }
        public DateTime? ActualCheckOut { get; set; }

        public int Adults { get; set; } = 1;
        public int Children { get; set; }

        public int RatePlanId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DepositAmount { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
        public BookingSource Source { get; set; } = BookingSource.Direct;

        public string? SpecialRequest { get; set; }
        public string? Notes { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Optimistic concurrency token — chống double booking.</summary>
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Navigation
        public Guest Guest { get; set; } = null!;
        public RoomType RoomType { get; set; } = null!;
        public Room? Room { get; set; }
        public RatePlan RatePlan { get; set; } = null!;
        public ICollection<Folio> Folios { get; set; } = new List<Folio>();
        public ICollection<ReservationLog> Logs { get; set; } = new List<ReservationLog>();
    }
}
