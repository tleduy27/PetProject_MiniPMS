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

        // Bảng chuyển trạng thái hợp lệ: từ -> các đích cho phép
        private static readonly Dictionary<RoomStatus, RoomStatus[]> _allowed = new()
        {
            [RoomStatus.Available] = new[] { RoomStatus.Occupied, RoomStatus.OutOfOrder, RoomStatus.OutOfService },
            [RoomStatus.Occupied] = new[] { RoomStatus.Available },                       // phải check-out trước
            [RoomStatus.OutOfOrder] = new[] { RoomStatus.Available, RoomStatus.OutOfService },
            [RoomStatus.OutOfService] = new[] { RoomStatus.Available, RoomStatus.OutOfOrder },
        };
        public bool CanChangeTo(RoomStatus next) =>
    Status == next || (_allowed.TryGetValue(Status, out var targets) && targets.Contains(next));
    }
    }
