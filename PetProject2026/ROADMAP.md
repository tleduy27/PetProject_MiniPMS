# 🗺️ Roadmap Backend — Mini PMS (Fresher → Middle .NET)

> Mục tiêu: vừa hoàn thiện petproject để đi phỏng vấn, vừa củng cố kiến thức .NET backend
> theo lộ trình từ Fresher đến Middle. Mỗi phase = 1 cụm tính năng + 1 cụm kiến thức.
>
> Quy ước: ✅ Done · 🔵 In progress · ⬜ To do

---

## Phase 0 — Nền móng (✅ Done)

**Đã làm:**
- Chuẩn hóa Domain: Entities (số ít, enum, navigation properties), `Domain/Enums`, `Domain/Interfaces`
- EF Core Code First: `AppDbContext`, Fluent API config, enum lưu dạng string, precision decimal
- Generic `Repository<T>` + `IUnitOfWork`
- Migration `InitialCreate` + `SeedData` (RoomType, Room, RatePlan, RoomRate, Guest)

**Kiến thức củng cố:**
- EF Core Code First, Fluent API vs Data Annotations
- Navigation properties, Delete Behavior (Cascade/Restrict)
- Migration workflow (`dotnet ef migrations add/update`)

---

## Phase 1 — CQRS Pipeline + Guest module (🔵 In progress)

**Đã làm:**
- Cài MediatR + FluentValidation
- `GetRoomsQuery` (Query mẫu — projection, không Include)
- `CreateGuestCommand` + `CreateGuestCommandValidator`
- `ValidationBehavior` (MediatR Pipeline Behavior)
- `ExceptionHandlingMiddleware` (Global exception handling → 400 chuẩn)

**Còn lại:**
- ⬜ `GetGuestByIdQuery` — học cách xử lý **"not found"**:
  - Tạo `Domain/Exceptions/NotFoundException.cs`
  - Thêm `catch (NotFoundException)` → 404 trong middleware
- ⬜ `GetGuestsQuery` (search theo tên/sđt, có phân trang — học `Skip`/`Take`, `PagedResult<T>`)
- ⬜ `UpdateGuestCommand`

**Kiến thức củng cố:**
- CQRS, MediatR (Command/Query/Handler/PipelineBehavior)
- FluentValidation, Validation Pipeline
- Global Exception Handling Middleware
- DTO vs Entity, Projection (`Select`) vs `Include`
- Pagination pattern

---

## Phase 2 — Room & Rate Management (⬜ To do)

**Tính năng:**
- `RoomTypesController`: CRUD RoomType
- `RoomsController`: CRUD Room, `UpdateRoomStatusCommand` (đổi Status/HousekeepingStatus)
- `GetAvailableRoomsQuery`: phòng trống theo `RoomTypeId` + khoảng ngày — **nền tảng cho Reservation**
- `RatePlansController` + `RoomRatesController`: CRUD giá theo ngày

**Kiến thức củng cố:**
- Tách rõ Command (ghi) vs Query (đọc) cho CRUD đầy đủ
- Viết query "tìm phòng trống" — kỹ thuật kiểm tra **overlap khoảng thời gian**:
  ```
  KHÔNG (existing.CheckOut <= new.CheckIn OR existing.CheckIn >= new.CheckOut)
  ```
- AutoMapper (cân nhắc thay thế map thủ công bằng `Select` — so sánh ưu/nhược)

---

## Phase 3 — Reservation (⭐ Trọng tâm phỏng vấn) (⬜ To do)

**Tính năng:**
1. `CreateReservationCommand`
   - Validate: `CheckInDate < CheckOutDate`, `Adults >= 1`, ngày không ở quá khứ
   - Check phòng trống theo `RoomTypeId` (dùng lại logic Phase 2)
   - Sinh `ReservationNumber` (vd: `RSV-20260611-0001`)
   - Tạo `Reservation` với `Status = Confirmed`
   - Ghi `ReservationLog` (ChangeType = "Create")
2. `GetReservationByIdQuery`, `SearchReservationsQuery` (lọc theo ngày, status, guest)
3. `CheckInCommand`
   - Gán `RoomId`, `ActualCheckIn = now`, `Status = CheckedIn`
   - Đổi `Room.Status = Occupied`
   - Tạo `Folio` (FolioType = Guest, Status = Open)
   - Ghi `ReservationLog`
4. `CheckOutCommand`
   - Kiểm tra Folio balance == 0 (chưa nợ tiền)
   - `Status = CheckedOut`, `ActualCheckOut = now`
   - `Room.Status = Available`, `Room.HousekeepingStatus = Dirty`
   - Đóng Folio (`Status = Closed`)
5. `CancelReservationCommand`
   - Chỉ cho cancel khi `Status == Confirmed`
   - Áp dụng cancellation policy (đơn giản: tính phí nếu hủy < X giờ trước CheckIn)

**Kiến thức củng cố — đây là phần "ăn điểm" nhất khi phỏng vấn:**
- **Optimistic Concurrency** (`RowVersion`) — chống 2 lễ tân check-in cùng 1 phòng cùng lúc
  - Test: 2 request đồng thời → 1 cái phải nhận `DbUpdateConcurrencyException`
- **Domain validation vs Application validation** — phân biệt rule nào ở Entity, rule nào ở Validator/Handler
- **State machine** cho `ReservationStatus` (chỉ cho phép transition hợp lệ: Confirmed→CheckedIn→CheckedOut, không cho CheckedOut→Confirmed)
- Transaction — đảm bảo Create Reservation + Folio + Log là 1 unit (dùng `SaveChangesAsync` 1 lần, EF tự transaction)

---

## Phase 4 — Folio & Cashier (⬜ To do)

**Tính năng:**
- `PostChargeCommand` — thêm `FolioTransaction` (Charge) vào folio (vd: minibar, dịch vụ)
- `PostPaymentCommand` — thêm `Payment`, validate `Amount > 0`
- `GetFolioQuery` — trả về folio + tổng hợp: `TotalCharges`, `TotalPayments`, `Balance`
- `VoidTransactionCommand` — set `VoidedAt`/`VoidedBy`, không xóa record (audit trail)

**Kiến thức củng cố:**
- Tính toán aggregate (SUM) bằng LINQ trên DB (`SumAsync`)
- Soft delete / Void pattern (giữ lịch sử thay vì xóa)
- Money/Decimal precision, làm tròn

---

## Phase 5 — Domain Events (⬜ To do)

**Tính năng:**
- Khi `CheckOutCommand` thành công → raise `ReservationCheckedOutEvent`
- Handler của event này tự động:
  - Set `Room.HousekeepingStatus = Dirty`
  - (Optional) Tạo `HousekeepingAssignment` tự động

**Kiến thức củng cố:**
- MediatR `INotification` + `INotificationHandler<T>` (khác với `IRequest`/`IRequestHandler`)
- Tách side-effect ra khỏi Command Handler chính → Single Responsibility
- So sánh: gọi trực tiếp vs raise Domain Event — khi nào nên dùng cái nào

---

## Phase 6 — Night Audit (Background Job) (⬜ To do)

**Tính năng:**
- Cài Hangfire (`Hangfire.AspNetCore`, `Hangfire.SqlServer`)
- `RunNightAuditCommand`:
  1. Tìm reservation `Confirmed` có `CheckInDate == BusinessDate` mà chưa check-in → đánh dấu `NoShow`
  2. Với mọi reservation `CheckedIn` → post `FolioTransaction` (ChargeCode = RoomCharge) = giá phòng đêm đó
  3. Tăng `BusinessDate` lên 1 ngày (lưu ở đâu? → bảng `SystemSettings` hoặc `BusinessDateConfig`)
- Lập lịch chạy lúc 23h (Hangfire Recurring Job) — hoặc expose API để demo chạy thủ công

**Kiến thức củng cố:**
- Background job / scheduled task (Hangfire)
- **Idempotency**: chạy Night Audit 2 lần cho cùng business date không được post charge 2 lần
- Khái niệm Business Date vs Calendar Date (đã ghi trong tài liệu nghiệp vụ)

---

## Phase 7 — Realtime (SignalR) (⬜ To do)

**Tính năng:**
- `RoomStatusHub` — khi `UpdateRoomStatusCommand`/`CheckInCommand`/`CheckOutCommand` chạy xong → broadcast `RoomStatusChanged(roomId, newStatus)`
- Frontend (sau này) subscribe để Floor Map tự đổi màu

**Kiến thức củng cố:**
- SignalR Hub, Group, `IHubContext<T>` inject vào Handler
- Real-time push vs polling

---

## Phase 8 — Reports (⬜ To do)

**Tính năng:**
- `GetOccupancyReportQuery` — tỷ lệ lấp đầy theo ngày/tháng
- `GetRevenueReportQuery` — doanh thu theo ngày, theo ChargeCode
- `GetArrivalDepartureListQuery`
- KPI: ADR, RevPAR (tính trong query hoặc service riêng)

**Kiến thức củng cố:**
- LINQ `GroupBy`, aggregate functions phức tạp
- Raw SQL / `FromSqlRaw` khi LINQ không tối ưu (so sánh performance)
- DTO cho report (khác DTO cho CRUD)

---

## Phase 9 — Authentication & Authorization (⬜ To do)

**Tính năng:**
- ASP.NET Identity hoặc custom User table
- JWT issue/validate
- Role: Admin, Receptionist, Cashier, Housekeeping, Manager (theo tài liệu nghiệp vụ)
- `[Authorize(Roles = "Receptionist,Manager")]` trên các Controller phù hợp

**Kiến thức củng cố:**
- JWT (claims, expiry, refresh token)
- Role-based vs Policy-based authorization
- `IHttpContextAccessor` để lấy `CurrentUserId` cho field `CreatedBy`/`PostedBy`

---

## Phase 10 — Testing (⬜ To do — nên làm song song từ Phase 3)

**Tính năng:**
- Unit test cho Handler (dùng `EF Core InMemory` hoặc mock `AppDbContext`)
- Test case quan trọng nhất: `CreateReservationCommandHandler` — phòng đã đầy → phải throw
- Integration test cho Validator

**Kiến thức củng cố:**
- xUnit, FluentAssertions, Moq (hoặc NSubstitute)
- Test pyramid: Unit vs Integration

---

## Phase 11 — Bonus (nếu còn thời gian) (⬜ To do)

- Group Booking + Master Folio (Phase 5 nghiệp vụ trong tài liệu)
- Cancellation Policy có cấu hình (bảng riêng, % phí theo số giờ trước check-in)
- Lost & Found CRUD
- Guest Request + SLA tracking (so sánh `ResolvedAt - RequestedAt` với SLA)
- Redis cache cho `GetAvailableRoomsQuery` (data đổi liên tục → cân nhắc TTL ngắn)

---

## Cách dùng file này

1. Mỗi lần bắt đầu 1 task, đổi trạng thái `⬜` → `🔵`, xong → `✅`.
2. Mỗi Phase nên kết thúc bằng: build sạch + test thủ công qua Swagger/curl + commit.
3. Phần "Kiến thức củng cố" là checklist để tự hỏi bản thân: *"Nếu phỏng vấn hỏi về cái này, mình giải thích được không?"*
4. Thứ tự Phase 3 → 6 là **lõi nghiệp vụ PMS** — ưu tiên cao nhất nếu thời gian hạn chế.
