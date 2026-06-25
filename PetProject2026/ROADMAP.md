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

## Phase 12 — Logging & Observability (⬜ To do — nên làm sớm, ROI cao)

**Tính năng:**
- Cài **Serilog** (`Serilog.AspNetCore`), cấu hình sink: Console + File (rolling theo ngày)
- **Structured logging**: log dạng object thay vì string nối chuỗi
  - VD: `_logger.LogInformation("Reservation {ReservationNumber} checked in to room {RoomId}", rsv.ReservationNumber, roomId)`
- **Correlation ID / Request ID**: middleware sinh `X-Correlation-Id` cho mỗi request, gắn vào mọi log dòng đó (dùng `LogContext.PushProperty`)
- Log enrichment: thêm `MachineName`, `RequestPath`, `UserId` (lấy từ `IHttpContextAccessor` sau Phase 9)
- Bổ sung log có chủ đích vào các Handler nghiệp vụ quan trọng: CheckIn, CheckOut, Night Audit, PostPayment
- (Optional) `/health` endpoint bằng `AspNetCore.HealthChecks` (check DB connection)

**Kiến thức củng cố:**
- Structured logging vs string logging — tại sao log dạng object dễ query (Seq/ELK)
- Log Level đúng chuẩn: Trace/Debug/Information/Warning/Error/Critical — khi nào dùng cái nào
- Correlation ID — cách trace 1 request xuyên nhiều layer/service
- Phân biệt logging với exception handling (đã có `ExceptionHandlingMiddleware` → log Error ở đây)

---

## Phase 13 — Docker (⬜ To do — gần như bắt buộc cho Middle)

**Tính năng:**
- Viết **`Dockerfile`** cho API (multi-stage build: `sdk` để build → `aspnet runtime` để chạy → image nhẹ)
- Viết **`docker-compose.yml`**: service `api` + service `sqlserver` (`mcr.microsoft.com/mssql/server`)
  - Dùng `depends_on`, `environment` cho connection string, `volumes` để persist data SQL
- Chuyển connection string ra biến môi trường (không hardcode trong `appsettings.json`)
- `dotnet ef database update` hoặc auto-migrate khi container khởi động (cân nhắc ưu/nhược)
- `.dockerignore` để loại `bin/`, `obj/`

**Kiến thức củng cố:**
- Image vs Container; multi-stage build — tại sao giảm được size image
- Cấu hình qua Environment Variables (12-factor app)
- Docker network — vì sao api gọi sqlserver bằng tên service chứ không phải `localhost`
- Volume — persist dữ liệu khi container bị xóa
- Câu hỏi hay gặp: "Khác nhau giữa `COPY` và `ADD`?", "`ENTRYPOINT` vs `CMD`?"

---

## Phase 14 — Redis (Caching) (⬜ To do — gộp luôn ý tưởng cache ở Phase 11)

**Tính năng:**
- Thêm service Redis vào `docker-compose`
- Cài `StackExchange.Redis` + `Microsoft.Extensions.Caching.StackExchangeRedis` (`IDistributedCache`)
- Áp dụng **Cache-Aside pattern** cho `GetAvailableRoomsQuery` và `GetRoomTypesQuery` (data ít đổi):
  1. Đọc cache → có thì trả luôn
  2. Miss → query DB → ghi vào cache với **TTL ngắn** (vd 30–60s vì phòng trống đổi liên tục)
- **Cache invalidation**: khi `UpdateRoomStatusCommand` / `CheckInCommand` / `CheckOutCommand` chạy → xóa key cache liên quan
- (Optional) Dùng Redis làm distributed lock cho check-in (thay/bổ sung cho Optimistic Concurrency ở Phase 3)

**Kiến thức củng cố:**
- Cache-Aside vs Write-Through vs Write-Behind
- **Cache invalidation** — "thứ khó thứ 2 trong CS"; chiến lược nào cho data hay đổi
- TTL, cân bằng giữa độ tươi của data và hiệu năng
- In-memory cache (`IMemoryCache`) vs Distributed cache (Redis) — khi nào cần distributed
- Cache stampede / thundering herd — vấn đề khi nhiều request cùng miss 1 lúc

---

## Phase 15 — Message Queue (⬜ To do — điểm cộng, làm chọn lọc)

**Tính năng:**
- Thêm **RabbitMQ** vào `docker-compose`. Cài `MassTransit` (`MassTransit.RabbitMQ`) — abstraction dễ dùng hơn client thuần
- Tách 1 side-effect ra xử lý **bất đồng bộ** thay vì gọi trực tiếp:
  - VD: Sau `CheckOutCommand` → publish message `ReservationCheckedOut` → Consumer gửi email hóa đơn / cập nhật report
  - (Liên kết với Phase 5 Domain Events: Domain Event trong process vs Integration Event qua queue)
- Xử lý **retry** + **dead-letter queue** khi consumer fail

**Kiến thức củng cố:**
- Khi nào dùng Message Queue (decoupling, load leveling, async) vs gọi trực tiếp HTTP/method
- Producer/Consumer, Exchange/Queue/Routing key (RabbitMQ)
- **At-least-once delivery** → consumer phải **idempotent** (liên kết Phase 6 Night Audit)
- Domain Event (in-process, MediatR) vs Integration Event (cross-service, MQ) — khác nhau chỗ nào
- ⚠️ Chỉ cần 1 luồng demo chạy được + giải thích được trade-off; đừng over-engineer

---

## Phase 16 — gRPC (⬜ Optional — ROI thấp ở mức 20tr, để cuối)

**Tính năng:**
- Tạo 1 `.proto` đơn giản, vd `RoomService` với method `GetRoomStatus(roomId)`
- Tách 1 service nhỏ expose qua gRPC, hoặc thêm gRPC endpoint song song REST trong cùng app
- Gọi thử bằng client (Postman hỗ trợ gRPC hoặc viết console client)

**Kiến thức củng cố:**
- gRPC vs REST: Protobuf (binary) vs JSON, HTTP/2, streaming
- Khi nào chọn gRPC (giao tiếp service-to-service, latency thấp, payload lớn) vs REST (public API, browser)
- Contract-first với file `.proto`
- ⚠️ Phỏng vấn 20tr hiếm khi đào sâu — biết khái niệm + làm được 1 demo là đủ

---

## Phase 17 — CI/CD (⬜ To do — dễ ghi điểm, làm nhanh)

**Tính năng:**
- Viết **GitHub Actions workflow** (`.github/workflows/ci.yml`):
  1. Trigger: push / pull_request lên `master`
  2. `dotnet restore` → `dotnet build` → `dotnet test` (chạy unit test Phase 10)
  3. (CD) `docker build` + push image lên Docker Hub / GitHub Container Registry
- Thêm **badge** build status vào `README`
- (Optional) Tách stage: build → test → publish; chỉ deploy khi merge vào `master`

**Kiến thức củng cố:**
- CI vs CD — khác nhau chỗ nào
- Pipeline stages, fail-fast (build/test fail thì không deploy)
- Secrets management (không commit credential — dùng GitHub Secrets)
- Vì sao cần test tự động trong pipeline (chống regression)

---

## Phase 18 — Clean Architecture nâng cao (⬜ To do — đừng over-engineer)

> Lưu ý: project hiện đã có Domain/Application(CQRS)/Infrastructure/API. Phase này là **soi lại & giải thích được**, không phải đập đi xây lại.

**Tính năng / Refactor:**
- Rà soát **dependency direction**: Domain không phụ thuộc EF Core/ASP.NET; Application chỉ biết Interface, không biết implementation
- Đưa các interface (vd `IUnitOfWork`, `IRepository`, `ICurrentUser`) về đúng layer (Application/Domain), implementation ở Infrastructure
- Cân nhắc tách `Result<T>` pattern (thay vì throw exception cho luồng nghiệp vụ bình thường) — so sánh ưu/nhược với Exception
- Đảm bảo Controller "mỏng": chỉ nhận request → gọi MediatR → trả response

**Kiến thức củng cố:**
- **Dependency Inversion Principle** trong thực tế (chiều phụ thuộc trỏ vào trong)
- Vì sao Domain phải "sạch", không dính framework
- Clean Architecture vs Onion vs Hexagonal — điểm chung
- Trade-off: khi nào Clean Architecture là over-engineering (project nhỏ)
- Câu hỏi phỏng vấn: "Nếu đổi từ SQL Server sang Postgres / từ EF sang Dapper thì sửa ở đâu?"

---

## Phase 19 — Production Issues (⬜ Xuyên suốt — gắn vào từng phase, không để cuối)

> "Vàng" khi phỏng vấn Middle: kể được 1 vấn đề thật bạn gặp + cách tìm ra + cách fix.

**Chủ đề thực hành ngay trên project:**
- **N+1 query**: cố tình tạo N+1 ở `SearchReservationsQuery` (lặp truy cập navigation property) → bật log SQL của EF để thấy → fix bằng `Include`/projection. **Phải tự reproduce được.**
- **Tracking vs No-Tracking**: query chỉ-đọc (report, list) phải `AsNoTracking()` — đo khác biệt
- **Deadlock / Concurrency**: 2 request check-in cùng phòng (đã đụng ở Phase 3) — quan sát `DbUpdateConcurrencyException`
- **Connection pool exhaustion**: hiểu vì sao quên `await`/quên dispose gây cạn pool
- **Transaction isolation**: đọc bẩn (dirty read), lost update — minh họa bằng Night Audit chạy trùng
- **Memory leak cơ bản**: static event handler, `IDisposable` không dispose

**Kiến thức củng cố:**
- Cách **đọc SQL EF sinh ra** (log / `ToQueryString()`) — kỹ năng debug số 1
- Đọc execution plan đơn giản, biết khi nào thiếu index
- Cách tiếp cận điều tra: reproduce → log/metrics → tìm root cause → fix → verify

---

## Phase 20 — System Design cơ bản (⬜ To do — học bằng vẽ, không code trong project)

> Bắt buộc cho Middle. Mục tiêu: vẽ + giải thích được, không phải code thêm.

**Bài tập vẽ (dùng chính PMS làm đề):**
- Vẽ kiến trúc hệ thống PMS hiện tại: Client → API → DB, thêm Redis, MQ, background job (Hangfire) vào đúng chỗ
- "Nếu PMS phục vụ **chuỗi 500 khách sạn** thì thiết kế lại thế nào?"
  - Scale: load balancer, nhiều instance API (stateless), read replica DB
  - Caching layer, CDN cho static
  - Tách service (Reservation / Folio / Reporting) — khi nào nên tách microservice
- Thiết kế API "tìm phòng trống" chịu tải cao (cache + index + denormalization)

**Kiến thức củng cố:**
- Vertical vs Horizontal scaling; stateless service (vì sao quan trọng cho scale ngang)
- Load balancing, caching layer, DB read replica
- CAP theorem (mức khái niệm), eventual consistency
- SQL vs NoSQL — khi nào chọn cái nào
- Bottleneck thường gặp: DB là điểm nghẽn đầu tiên → cache + index + đọc/ghi tách
- Cách trình bày 1 bài system design: clarify requirement → ước lượng tải → vẽ high-level → đi sâu component

---

## 🧠 Nền tảng tự ôn (song song mọi Phase — KHÔNG phải phase code)

> Đây là phần phỏng vấn viên Middle **khoan sâu nhất** nhưng không gắn với feature nào.
> Không cần code thêm — mục tiêu là **giải thích trôi chảy + làm được vài đoạn demo nhỏ**.
> Tự chấm mỗi mục: nếu bị hỏi, mình đào sâu được mấy lớp câu hỏi?

### 1. Async / Await & Threading (⭐⭐⭐ hay hỏi nhất)
- `Task` vs `Thread` vs `ValueTask`; `async/await` thực sự làm gì (state machine)
- **Deadlock kinh điển**: `.Result` / `.Wait()` trên async code → vì sao treo; cách tránh
- `ConfigureAwait(false)` — khi nào cần, vì sao trong ASP.NET Core thường không quan trọng
- `Task.WhenAll` vs `await` tuần tự — chạy song song nhiều việc I/O
- `CancellationToken` — truyền xuyên Handler/Repository (project bạn nên truyền sẵn)
- `IAsyncEnumerable` — stream data lớn
- ⚠️ Tự reproduce 1 deadlock rồi fix — kể được trong phỏng vấn là ăn điểm

### 2. SQL thuần & Database (⭐⭐⭐ EF Core KHÔNG thay thế được)
- **Index**: clustered vs non-clustered, covering index, khi nào index *làm chậm* (ghi nhiều)
- Đọc **execution plan** đơn giản — nhận ra table scan vs index seek
- **Transaction isolation levels**: Read Uncommitted → Serializable; dirty/non-repeatable/phantom read
- **Deadlock** ở DB — nguyên nhân (thứ tự lock khác nhau) và cách giảm
- JOIN (inner/left/cross), `GROUP BY`, window function (`ROW_NUMBER`, `RANK`) — dùng cho report Phase 8
- `EXISTS` vs `IN` vs `JOIN`; N+1 nhìn từ phía SQL
- Luyện: viết tay query "tìm phòng trống overlap ngày" bằng SQL thuần (không EF)

### 3. OOP / SOLID / Design Patterns (⭐⭐ hay bắt vẽ + giải thích)
- 4 tính chất OOP — giải thích bằng ví dụ trong project, không học vẹt
- **SOLID** — mỗi nguyên lý 1 ví dụ thật từ code bạn (vd: Repository = DIP, Handler tách = SRP)
- Pattern bạn ĐANG dùng, phải giải thích được *trade-off*:
  - Repository + Unit of Work (vì sao dùng / khi nào thừa với EF)
  - Mediator (MediatR), Pipeline Behavior (Validation)
  - Strategy (cancellation policy Phase 3), Factory, Decorator
- Composition over Inheritance — khi nào ưu tiên

### 4. EF Core nâng cao (⭐⭐⭐ gắn chặt project)
- **Change Tracking** — `AsNoTracking()` cho query đọc; vì sao tăng tốc
- `IQueryable` vs `IEnumerable` — thời điểm query bị **materialize** (đẩy xuống DB vs chạy trên RAM)
- **N+1** — nhận diện, fix bằng `Include` / projection `Select` (đã đụng ở Phase 8, 19)
- Lazy vs Eager vs Explicit loading
- `Include` quá nhiều → cartesian explosion → `AsSplitQuery()`
- Optimistic Concurrency (`RowVersion`) — đã có ở Phase 3, ôn lại
- Migration trong team thật: conflict, rollback, không sửa migration đã apply

### 5. ASP.NET Core nội tại (⭐⭐)
- **Middleware pipeline** — thứ tự chạy, `next()`, vì sao Exception middleware đặt đầu (bạn đã có)
- **Dependency Injection**: `Scoped` vs `Transient` vs `Singleton` — **bẫy**: inject Scoped (DbContext) vào Singleton → lỗi
- Model binding, filter, `IActionResult` vs `ActionResult<T>`
- Kestrel, `appsettings` theo môi trường, `IOptions<T>`

### 6. HTTP / REST / API design (⭐⭐)
- Status code đúng chuẩn (200/201/204/400/401/403/404/409/422)
- **Idempotency** (đã có Phase 6) — method nào idempotent, vì sao
- REST vs RPC; versioning API; pagination/filtering chuẩn (đã có Phase 1)
- Authentication vs Authorization (đào sâu sau Phase 9)

### 7. Git & teamwork (⭐ nhưng dễ mất điểm nếu lúng túng)
- merge vs rebase, giải quyết conflict, `cherry-pick`
- Branch strategy (Git Flow / trunk-based), pull request review
- `git reset` vs `revert` — cái nào an toàn trên branch đã push

> **Cách dùng phần này:** mỗi tuần chọn 1–2 mục, viết câu trả lời ra giấy như đang phỏng vấn,
> rồi tự hỏi ngược "tại sao?" 3 lần. Mục nào trả lời được < 2 lớp → đánh dấu cần ôn lại.

---

## Cách dùng file này

1. Mỗi lần bắt đầu 1 task, đổi trạng thái `⬜` → `🔵`, xong → `✅`.
2. Mỗi Phase nên kết thúc bằng: build sạch + test thủ công qua Swagger/curl + commit.
3. Phần "Kiến thức củng cố" là checklist để tự hỏi bản thân: *"Nếu phỏng vấn hỏi về cái này, mình giải thích được không?"*
4. Thứ tự Phase 3 → 6 là **lõi nghiệp vụ PMS** — ưu tiên cao nhất nếu thời gian hạn chế.
