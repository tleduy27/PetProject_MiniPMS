namespace PetProject2026.Domain.Enums
{
    /// <summary>Vòng đời của một booking.</summary>
    public enum ReservationStatus
    {
        Confirmed = 0,
        CheckedIn = 1,
        CheckedOut = 2,
        Cancelled = 3,
        NoShow = 4
    }
}
