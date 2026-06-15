namespace PetProject2026.Application.Common.DTOs
{
    public record RoomTypeDto
    (
        int Id,
        string Name,
        string? Description,
        int MaxOccupancy,
        decimal BasePrice,
        string? Amenities
    );
}
