using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Application.Common.DTOs;
using PetProject2026.Application.Common.Models;
using PetProject2026.Infrastructure.Persistence;

namespace PetProject2026.Application.Rooms.Queries
{
    /// <summary>Lấy danh sách toàn bộ phòng kèm tên loại phòng.</summary> - Toi la mot request va khi xu li xong toi tra ve list<roomDto></roomDto>
    public record GetRoomsQuery(string? Search = null, int PageNumber = 1, int PageSize = 10) : IRequest<PagedResult<RoomDto>>;
    /// <summary>
    /// GetRoomsQuerhyHandler la handler xu li getRoomQuery va tra ve List<RoomDto></RoomDto>
    /// </summary>
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, PagedResult<RoomDto>>
    {
        private readonly AppDbContext _db;

        public GetRoomsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<RoomDto>> Handle(GetRoomsQuery request, CancellationToken ct)
        {
            //1. CHan gia tri vo ly (trang<1, size <=0 hoặc quá lớn)
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize is < 1 or > 100 ? 10 : request.PageSize;
            //2. bat dau dung cau truy van
            var query = _db.Rooms.AsNoTracking();
            //3.ap filter search co dieu kien: chi them where khi co tu khoa
            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                var keyword = request.Search.Trim();
                query = query.Where(r => r.RoomNumber.Contains(keyword));
            }
            //4.Dem tong so ban ghi sau khi filter
            var totalCount = await query.CountAsync(ct);
            //5.Sap xep -> skip (bo qua cac trang truoc) -> take (lay dung 1 trang)
            //luon can orderby truoc skip/take, neu khong thu tu trang khong on dinh
            var items = await query.OrderBy(r => r.RoomNumber).Skip((pageNumber-1)*pageSize).Take(pageSize).Select(r => new RoomDto(
                r.Id,
                r.RoomNumber,
                r.Floor,
                r.RoomTypeId,
                r.RoomType.Name,
                r.Status.ToString(),
                r.HousekeepingStatus.ToString(),
                r.Notes
                )).ToListAsync();
            return new PagedResult<RoomDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            
        }
    }
}
