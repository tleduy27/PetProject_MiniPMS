using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Phòng vật lý thực tế (101, 202...).</summary>
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public HousekeepingStatus HousekeepingStatus { get; set; } = HousekeepingStatus.Clean;
        public string? Notes { get; set; }

        // Navigation
        public RoomType RoomType { get; set; } = null!;
    }
}
