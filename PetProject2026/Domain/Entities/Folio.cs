using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Tài khoản thanh toán của khách trong một lần lưu trú.</summary>
    public class Folio
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public FolioType FolioType { get; set; } = FolioType.Guest;
        public FolioStatus Status { get; set; } = FolioStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Reservation Reservation { get; set; } = null!;
        public ICollection<FolioTransaction> Transactions { get; set; } = new List<FolioTransaction>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
