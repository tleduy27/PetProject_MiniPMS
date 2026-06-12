namespace PetProject2026.Domain.Exception
{
    /// <summary>
    /// Ném ra khi một tài nguyên được yêu cầu không tồn tại.
    /// Kế thừa System.Exception để middleware có thể bắt và map sang HTTP 404.
    /// </summary>
    public class NotFoundException : System.Exception
    {
        // 1) Constructor mặc định - cho phép "throw new NotFoundException();"
        public NotFoundException()
        {
        }

        // 2) Chỉ kèm message - dùng khi muốn tự viết nội dung lỗi.
        public NotFoundException(string message)
            : base(message)
        {
        }

        // 3) Message + inner exception - dùng khi bọc (wrap) một lỗi gốc khác,
        //    giữ lại nguyên nhân thật để debug.
        public NotFoundException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        // 4) Constructor tiện ích - tự sinh message chuẩn:
        //    new NotFoundException("Guest", 5) -> "Guest with id 5 was not found."
        //    Lưu ý: 'key' là object nên không đụng với constructor (3) vì int không phải Exception.
        public NotFoundException(string name, object key)
            : base($"{name} with id {key} was not found.")
        {
        }
    }
}
