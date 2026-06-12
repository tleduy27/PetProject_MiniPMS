namespace PetProject2026.Domain.Entities
{
    /// <summary>Nhật ký thay đổi của một booking.</summary>
    public class ReservationLog
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public string? ChangedBy { get; set; }
        public string ChangeType { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Reservation Reservation { get; set; } = null!;
    }
}
