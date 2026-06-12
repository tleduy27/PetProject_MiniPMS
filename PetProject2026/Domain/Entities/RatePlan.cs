namespace PetProject2026.Domain.Entities
{
    /// <summary>Gói giá (Room only, B&amp;B, Half board...).</summary>
    public class RatePlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsIncludeBreakfast { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<RoomRate> RoomRates { get; set; } = new List<RoomRate>();
    }
}
