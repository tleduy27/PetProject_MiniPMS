namespace PetProject2026.Domain.Interfaces
{
    /// <summary>Commit thay đổi trong một transaction logic.</summary>
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
