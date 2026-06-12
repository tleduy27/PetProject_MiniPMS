using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Application.Common.Models;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Guests.Queries
{
    /// <summary>
    /// Tìm kiếm khách theo tên/sđt, có phân trang.
    /// Search = null/rỗng -> lấy tất cả. PageNumber/PageSize có giá trị mặc định.
    /// </summary>
    public record GetGuestsQuery(string? Search = null, int PageNumber = 1, int PageSize = 10)
        : IRequest<PagedResult<GuestDto>>;

    public class GetGuestsQueryHandler : IRequestHandler<GetGuestsQuery, PagedResult<GuestDto>>
    {
        private readonly AppDbContext _db;

        public GetGuestsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<GuestDto>> Handle(GetGuestsQuery request, CancellationToken ct)
        {
            // 1) Chặn giá trị vô lý (trang < 1, size <= 0 hoặc quá lớn).
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;

            // 2) Bắt đầu từ IQueryable - CHƯA chạy SQL, mới chỉ "dựng" câu truy vấn.
            var query = _db.Guests.AsNoTracking();

            // 3) Áp filter search có ĐIỀU KIỆN: chỉ thêm Where khi có từ khóa.
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var keyword = request.Search.Trim();
                query = query.Where(g =>
                    g.FullName.Contains(keyword) ||
                    (g.Phone != null && g.Phone.Contains(keyword)));
            }

            // 4) Đếm TỔNG số bản ghi khớp filter (trước khi phân trang).
            //    Đây là 1 câu SQL COUNT riêng.
            var totalCount = await query.CountAsync(ct);

            // 5) Sắp xếp -> Skip (bỏ qua các trang trước) -> Take (lấy đúng 1 trang).
            //    LUÔN cần OrderBy trước Skip/Take, nếu không thứ tự trang không ổn định.
            var items = await query
                .OrderBy(g => g.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GuestDto(
                    g.Id,
                    g.FullName,
                    g.Email,
                    g.Phone,
                    g.IdType.ToString(),
                    g.Nationality,
                    g.DateOfBirth,
                    g.Address,
                    g.Notes,
                    g.CreatedAt
                ))
                .ToListAsync(ct);

            // 6) Đóng gói vào PagedResult.
            return new PagedResult<GuestDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
