namespace PetProject2026.Application.Common.DTOs
{
    public record RoomRateDto(int Id, int RoomTypeId, int RatePlanId, DateTime StartDate, DateTime EndDate, decimal Price);
}
