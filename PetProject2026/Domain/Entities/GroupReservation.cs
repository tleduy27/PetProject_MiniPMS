namespace PetProject2026.Domain.Entities
{
    /// <summary>Liên kết đoàn với từng booking.</summary>
    public class GroupReservation
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int ReservationId { get; set; }

        // Navigation
        public Group Group { get; set; } = null!;
        public Reservation Reservation { get; set; } = null!;
    }
}
