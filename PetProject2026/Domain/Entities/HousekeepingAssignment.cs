using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Phân công dọn phòng cho nhân viên buồng.</summary>
    public class HousekeepingAssignment
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public HousekeepingTaskStatus Status { get; set; } = HousekeepingTaskStatus.Pending;
        public string? Notes { get; set; }

        // Navigation
        public Room Room { get; set; } = null!;
    }
}
