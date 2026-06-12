using PetProject2026.Domain.Enums;

namespace PetProject2026.Domain.Entities
{
    /// <summary>Đoàn / công ty đặt nhiều phòng theo một hợp đồng chung.</summary>
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public int? CompanyId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int TotalRooms { get; set; }
        public GroupStatus Status { get; set; } = GroupStatus.Confirmed;
        public int? MasterFolioId { get; set; }
        public string? Notes { get; set; }

        // Navigation
        public Folio? MasterFolio { get; set; }
        public ICollection<GroupReservation> GroupReservations { get; set; } = new List<GroupReservation>();
    }
}
