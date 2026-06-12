namespace PetProject2026.Domain.Enums
{
    public enum IdType
    {
        NationalId = 0,  // CCCD/CMND
        Passport = 1
    }

    public enum RequestPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum GuestRequestStatus
    {
        Open = 0,
        InProgress = 1,
        Resolved = 2
    }
}
