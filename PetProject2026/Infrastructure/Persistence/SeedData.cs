using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Entities;
using PetProject2026.Domain.Enums;

namespace PetProject2026.Infrastructure.Persistence
{
    /// <summary>Dữ liệu mẫu nạp qua migration (HasData).</summary>
    public static class SeedData
    {
        private static readonly DateTime Seed = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static void Apply(ModelBuilder b)
        {
            b.Entity<RoomType>().HasData(
                new RoomType { Id = 1, Name = "Standard", Description = "Phòng tiêu chuẩn", MaxOccupancy = 2, BasePrice = 500_000m, Amenities = "TV, Wifi, Điều hòa" },
                new RoomType { Id = 2, Name = "Deluxe", Description = "Phòng cao cấp", MaxOccupancy = 3, BasePrice = 900_000m, Amenities = "TV, Wifi, Minibar, View" },
                new RoomType { Id = 3, Name = "Suite", Description = "Phòng hạng sang", MaxOccupancy = 4, BasePrice = 1_800_000m, Amenities = "Living room, Bathtub, Minibar" }
            );

            b.Entity<RatePlan>().HasData(
                new RatePlan { Id = 1, Name = "Room Only", Description = "Chỉ phòng", IsIncludeBreakfast = false, IsActive = true },
                new RatePlan { Id = 2, Name = "Bed & Breakfast", Description = "Phòng + ăn sáng", IsIncludeBreakfast = true, IsActive = true }
            );

            b.Entity<RoomRate>().HasData(
                new RoomRate { Id = 1, RoomTypeId = 1, RatePlanId = 1, StartDate = Seed, EndDate = Seed.AddYears(1), Price = 500_000m },
                new RoomRate { Id = 2, RoomTypeId = 1, RatePlanId = 2, StartDate = Seed, EndDate = Seed.AddYears(1), Price = 600_000m },
                new RoomRate { Id = 3, RoomTypeId = 2, RatePlanId = 1, StartDate = Seed, EndDate = Seed.AddYears(1), Price = 900_000m },
                new RoomRate { Id = 4, RoomTypeId = 2, RatePlanId = 2, StartDate = Seed, EndDate = Seed.AddYears(1), Price = 1_050_000m },
                new RoomRate { Id = 5, RoomTypeId = 3, RatePlanId = 1, StartDate = Seed, EndDate = Seed.AddYears(1), Price = 1_800_000m },
                new RoomRate { Id = 6, RoomTypeId = 3, RatePlanId = 2, StartDate = Seed, EndDate = Seed.AddYears(1), Price = 2_000_000m }
            );

            // 8 phòng trên 2 tầng
            b.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = "101", Floor = 1, RoomTypeId = 1, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Inspected },
                new Room { Id = 2, RoomNumber = "102", Floor = 1, RoomTypeId = 1, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Inspected },
                new Room { Id = 3, RoomNumber = "103", Floor = 1, RoomTypeId = 2, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Inspected },
                new Room { Id = 4, RoomNumber = "104", Floor = 1, RoomTypeId = 2, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Clean },
                new Room { Id = 5, RoomNumber = "201", Floor = 2, RoomTypeId = 1, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Inspected },
                new Room { Id = 6, RoomNumber = "202", Floor = 2, RoomTypeId = 2, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Inspected },
                new Room { Id = 7, RoomNumber = "203", Floor = 2, RoomTypeId = 3, Status = RoomStatus.Available, HousekeepingStatus = HousekeepingStatus.Inspected },
                new Room { Id = 8, RoomNumber = "204", Floor = 2, RoomTypeId = 3, Status = RoomStatus.OutOfOrder, HousekeepingStatus = HousekeepingStatus.Dirty, Notes = "Đang sửa điều hòa" }
            );

            b.Entity<Guest>().HasData(
                new Guest { Id = 1, FullName = "Nguyễn Văn An", Email = "an.nv@example.com", Phone = "0901234567", IdType = IdType.NationalId, IdNumber = "001099012345", Nationality = "Việt Nam", Address = "Hà Nội", CreatedAt = Seed },
                new Guest { Id = 2, FullName = "Trần Thị Bình", Email = "binh.tt@example.com", Phone = "0907654321", IdType = IdType.NationalId, IdNumber = "001199054321", Nationality = "Việt Nam", Address = "Đà Nẵng", CreatedAt = Seed }
            );
        }
    }
}
