namespace PetProject2026.Application.Common.DTOs
{
    /// <summary>Thông tin phòng hiển thị trên Floor Map / danh sách phòng.</summary>
    public record RoomDto(
        int Id,
        string RoomNumber,
        int Floor,
        int RoomTypeId,
        string RoomTypeName,
        string Status,
        string HousekeepingStatus,
        string? Notes
    );
}
