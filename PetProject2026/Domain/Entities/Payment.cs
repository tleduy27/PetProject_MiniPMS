using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Một lần thanh toán vào folio.</summary>
    public class Payment
    {
        public int Id { get; set; }
        public int FolioId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } = PaymentMethod.Cash;
        public string? Reference { get; set; }
        public string? ProcessedBy { get; set; }
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Folio Folio { get; set; } = null!;
    }
}
