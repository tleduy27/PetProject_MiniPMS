namespace PetProject2026.Domain.Enums
{
    /// <summary>Loại giao dịch trong folio.</summary>
    public enum TransactionType
    {
        Charge = 0,      // Cộng tiền vào folio
        Payment = 1,     // Khách thanh toán
        Deposit = 2,     // Tiền cọc
        Adjustment = 3,  // Điều chỉnh / giảm giá
        Transfer = 4     // Chuyển giữa các folio
    }

    public enum PaymentMethod
    {
        Cash = 0,
        Card = 1,
        BankTransfer = 2
    }
}
