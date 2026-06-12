using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetProject2026.Domain.Interfaces;

namespace PetProject2026.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;

        public Repository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
            => _set.FindAsync(new object?[] { id }, ct).AsTask();

        public async Task<IReadOnlyList<T>> ListAsync(CancellationToken ct = default)
            => await _set.AsNoTracking().ToListAsync(ct);

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => await _set.AsNoTracking().Where(predicate).ToListAsync(ct);

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _set.AddAsync(entity, ct);

        public void Update(T entity) => _set.Update(entity);

        public void Remove(T entity) => _set.Remove(entity);
    }
}
