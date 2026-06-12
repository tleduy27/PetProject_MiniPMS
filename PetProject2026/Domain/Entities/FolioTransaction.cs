using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Giao dịch (charge/payment...) trong một folio.</summary>
    public class FolioTransaction
    {
        public int Id { get; set; }
        public int FolioId { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? ChargeCode { get; set; }   // RoomCharge | FoodBeverage | Minibar...
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? PostedBy { get; set; }
        public DateTime? VoidedAt { get; set; }
        public string? VoidedBy { get; set; }

        // Navigation
        public Folio Folio { get; set; } = null!;
    }
}
