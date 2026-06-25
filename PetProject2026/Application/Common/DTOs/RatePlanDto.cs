namespace PetProject2026.Application.Common.DTOs
{
    public record RatePlanDto
    (
       int Id,
       string Name,
       string? Description,
       bool IsIncludeBreakfast,
       bool IsActive
    );
}
