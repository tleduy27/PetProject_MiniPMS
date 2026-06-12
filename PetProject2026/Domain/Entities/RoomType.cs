namespace PetProject2026.Domain.Entities
{
    /// <summary>Loại phòng (Standard, Deluxe, Suite...).</summary>
    public class RoomType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxOccupancy { get; set; }
        public decimal BasePrice { get; set; }
        public string? Amenities { get; set; }

        // Navigation
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<RoomRate> RoomRates { get; set; } = new List<RoomRate>();
    }
}
