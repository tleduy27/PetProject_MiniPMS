namespace PetProject2026.Application.Common.Models
{
    /// <summary>
    /// Kết quả phân trang dùng chung cho mọi loại dữ liệu (T = GuestDto, RoomDto, ...).
    /// Ngoài danh sách Items còn kèm metadata để client biết tổng số trang, có trang sau/trước không.
    /// </summary>
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = new List<T>();

        public int TotalCount { get; init; }   // Tổng số bản ghi khớp điều kiện (KHÔNG phải số item trên trang)
        public int PageNumber { get; init; }   // Trang hiện tại (bắt đầu từ 1)
        public int PageSize { get; init; }      // Số item mỗi trang

        // Các thuộc tính tính toán (computed) - không lưu, tự suy ra từ dữ liệu trên.
        public int TotalPages => (int)System.Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}
