using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public abstract class Repository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>()
                .Where(expression);
        }
    }
}