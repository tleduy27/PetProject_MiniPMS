namespace PetProject2026.Domain.Exception
{
    /// <summary>
    /// Ném khi thao tác vi phạm quy tắc nghiệp vụ (vd: chuyển trạng thái không hợp lệ).
    /// Middleware map sang HTTP 409 Conflict.
    /// </summary>
    public class ConflictException : System.Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
