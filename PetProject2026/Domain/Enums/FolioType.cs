namespace PetProject2026.Domain.Enums
{
    public enum FolioType
    {
        Guest = 0,    // Folio cá nhân của khách
        Master = 1,   // Folio tổng của đoàn / công ty
        Virtual = 2   // Phòng ảo
    }

    public enum FolioStatus
    {
        Open = 0,
        Closed = 1
    }
}
