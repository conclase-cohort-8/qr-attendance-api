using QrAttendanceApi.Domain.Entities;
using System.Linq.Expressions;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool track = false);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
    }
}
