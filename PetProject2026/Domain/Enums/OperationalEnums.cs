namespace PetProject2026.Domain.Enums
{
    public enum HousekeepingTaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Done = 2
    }

    public enum LostFoundStatus
    {
        Stored = 0,
        Claimed = 1,
        Disposed = 2
    }

    public enum GroupStatus
    {
        Confirmed = 0,
        InHouse = 1,
        CheckedOut = 2,
        Cancelled = 3
    }

    /// <summary>Nguồn đặt phòng.</summary>
    public enum BookingSource
    {
        Direct = 0,
        WalkIn = 1,
        Booking = 2,   // Booking.com
        Agoda = 3,
        Corporate = 4,
        Other = 5
    }
}
