namespace PetProject2026.Domain.Enums
{
    /// <summary>Trạng thái kỹ thuật của phòng vật lý.</summary>
    public enum RoomStatus
    {
        Available = 0,
        Occupied = 1,
        OutOfOrder = 2,   // OOO - ngừng sử dụng (sửa chữa)
        OutOfService = 3  // OOS - tạm không phục vụ
    }
}
