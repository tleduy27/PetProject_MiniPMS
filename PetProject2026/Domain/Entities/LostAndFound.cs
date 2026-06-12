using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Đồ khách bỏ quên (Lost &amp; Found).</summary>
    public class LostAndFound
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public DateTime FoundDate { get; set; } = DateTime.UtcNow;
        public string? FoundBy { get; set; }
        public string? ClaimedBy { get; set; }
        public DateTime? ClaimedDate { get; set; }
        public LostFoundStatus Status { get; set; } = LostFoundStatus.Stored;

        // Navigation
        public Room Room { get; set; } = null!;
    }
}
