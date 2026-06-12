using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Interfaces;

namespace PetProject2026.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RatePlan> RatePlans => Set<RatePlan>();
        public DbSet<RoomRate> RoomRates => Set<RoomRate>();

        public DbSet<Guest> Guests => Set<Guest>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<ReservationLog> ReservationLogs => Set<ReservationLog>();

        public DbSet<Folio> Folios => Set<Folio>();
        public DbSet<FolioTransaction> FolioTransactions => Set<FolioTransaction>();
        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<HousekeepingAssignment> HousekeepingAssignments => Set<HousekeepingAssignment>();
        public DbSet<LostAndFound> LostAndFounds => Set<LostAndFound>();
        public DbSet<GuestRequest> GuestRequests => Set<GuestRequest>();

        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupReservation> GroupReservations => Set<GroupReservation>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // ---- Lưu enum dưới dạng chuỗi cho dễ đọc trong DB ----
            ConfigureEnumsAsString(b);

            // ---- Precision tiền tệ ----
            foreach (var prop in b.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                prop.SetPrecision(18);
                prop.SetScale(2);
            }

            // ---- RoomType ----
            b.Entity<RoomType>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.Property(x => x.Amenities).HasMaxLength(500);
            });

            // ---- Room ----
            b.Entity<Room>(e =>
            {
                e.Property(x => x.RoomNumber).HasMaxLength(20).IsRequired();
                e.HasIndex(x => x.RoomNumber).IsUnique();
                e.HasOne(x => x.RoomType).WithMany(t => t.Rooms)
                    .HasForeignKey(x => x.RoomTypeId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- RatePlan ----
            b.Entity<RatePlan>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
            });

            // ---- RoomRate ----
            b.Entity<RoomRate>(e =>
            {
                e.HasOne(x => x.RoomType).WithMany(t => t.RoomRates)
                    .HasForeignKey(x => x.RoomTypeId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.RatePlan).WithMany(p => p.RoomRates)
                    .HasForeignKey(x => x.RatePlanId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- Guest ----
            b.Entity<Guest>(e =>
            {
                e.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                e.Property(x => x.Email).HasMaxLength(150);
                e.Property(x => x.Phone).HasMaxLength(30);
                e.Property(x => x.IdNumber).HasMaxLength(50);
                e.Property(x => x.Nationality).HasMaxLength(80);
                e.Property(x => x.Address).HasMaxLength(300);
            });

            // ---- Reservation ----
            b.Entity<Reservation>(e =>
            {
                e.Property(x => x.ReservationNumber).HasMaxLength(30).IsRequired();
                e.HasIndex(x => x.ReservationNumber).IsUnique();
                e.HasOne(x => x.Guest).WithMany(g => g.Reservations)
                    .HasForeignKey(x => x.GuestId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.RoomType).WithMany()
                    .HasForeignKey(x => x.RoomTypeId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Room).WithMany()
                    .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.RatePlan).WithMany()
                    .HasForeignKey(x => x.RatePlanId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- ReservationLog ----
            b.Entity<ReservationLog>(e =>
            {
                e.HasOne(x => x.Reservation).WithMany(r => r.Logs)
                    .HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Cascade);
            });

            // ---- Folio ----
            b.Entity<Folio>(e =>
            {
                e.HasOne(x => x.Reservation).WithMany(r => r.Folios)
                    .HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Cascade);
            });

            // ---- FolioTransaction ----
            b.Entity<FolioTransaction>(e =>
            {
                e.Property(x => x.ChargeCode).HasMaxLength(50);
                e.Property(x => x.Description).HasMaxLength(300);
                e.HasOne(x => x.Folio).WithMany(f => f.Transactions)
                    .HasForeignKey(x => x.FolioId).OnDelete(DeleteBehavior.Cascade);
            });

            // ---- Payment ----
            b.Entity<Payment>(e =>
            {
                e.Property(x => x.Reference).HasMaxLength(100);
                e.HasOne(x => x.Folio).WithMany(f => f.Payments)
                    .HasForeignKey(x => x.FolioId).OnDelete(DeleteBehavior.Cascade);
            });

            // ---- HousekeepingAssignment ----
            b.Entity<HousekeepingAssignment>(e =>
            {
                e.Property(x => x.AssignedTo).HasMaxLength(100);
                e.HasOne(x => x.Room).WithMany()
                    .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- LostAndFound ----
            b.Entity<LostAndFound>(e =>
            {
                e.Property(x => x.ItemDescription).HasMaxLength(300).IsRequired();
                e.HasOne(x => x.Room).WithMany()
                    .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- GuestRequest ----
            b.Entity<GuestRequest>(e =>
            {
                e.Property(x => x.RequestType).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.HasOne(x => x.Room).WithMany()
                    .HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Reservation).WithMany()
                    .HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- Group ----
            b.Entity<Group>(e =>
            {
                e.Property(x => x.GroupName).HasMaxLength(150).IsRequired();
                e.Property(x => x.ContactPerson).HasMaxLength(150);
                e.HasOne(x => x.MasterFolio).WithMany()
                    .HasForeignKey(x => x.MasterFolioId).OnDelete(DeleteBehavior.Restrict);
            });

            // ---- GroupReservation ----
            b.Entity<GroupReservation>(e =>
            {
                e.HasOne(x => x.Group).WithMany(g => g.GroupReservations)
                    .HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Reservation).WithMany()
                    .HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);
            });

            SeedData.Apply(b);
        }

        private static void ConfigureEnumsAsString(ModelBuilder b)
        {
            b.Entity<Room>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Entity<Room>().Property(x => x.HousekeepingStatus).HasConversion<string>().HasMaxLength(20);
            b.Entity<Reservation>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Entity<Reservation>().Property(x => x.Source).HasConversion<string>().HasMaxLength(20);
            b.Entity<Guest>().Property(x => x.IdType).HasConversion<string>().HasMaxLength(20);
            b.Entity<Folio>().Property(x => x.FolioType).HasConversion<string>().HasMaxLength(20);
            b.Entity<Folio>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Entity<FolioTransaction>().Property(x => x.TransactionType).HasConversion<string>().HasMaxLength(20);
            b.Entity<Payment>().Property(x => x.Method).HasConversion<string>().HasMaxLength(20);
            b.Entity<HousekeepingAssignment>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Entity<LostAndFound>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Entity<GuestRequest>().Property(x => x.Priority).HasConversion<string>().HasMaxLength(20);
            b.Entity<GuestRequest>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            b.Entity<Group>().Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
            => base.SaveChangesAsync(ct);
    }
}
