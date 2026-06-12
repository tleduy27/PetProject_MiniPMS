namespace PetProject2026.Application.Common.DTOs
{
    public record GuestDto
    (
    int Id,
    string FullName,
    string? Email,
    string? Phone,
    string IdType,
    string? nationality,
    DateTime? DateOfBirth,
    string? Address,
    string? Notes,
    DateTime CreateAt
    );
        
    
}
