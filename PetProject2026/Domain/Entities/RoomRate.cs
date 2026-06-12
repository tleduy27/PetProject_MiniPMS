namespace PetProject2026.Domain.Entities
{
    /// <summary>Giá theo loại phòng + rate plan trong một khoảng ngày.</summary>
    public class RoomRate
    {
        public int Id { get; set; }
        public int RoomTypeId { get; set; }
        public int RatePlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        // Navigation
        public RoomType RoomType { get; set; } = null!;
        public RatePlan RatePlan { get; set; } = null!;
    }
}
