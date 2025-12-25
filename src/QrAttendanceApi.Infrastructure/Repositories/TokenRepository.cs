using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class TokenRepository : Repository<RefreshToken>, ITokenRepository
    {
        private readonly AppDbContext dbContext;

        public TokenRepository(AppDbContext dbContext) :
            base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(RefreshToken token, bool save = true)
        {
            await InsertAsync(token);
            if (save)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(RefreshToken token, bool save = true)
        {
            base.Update(token);
            if (save)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken?> FindAsync(Expression<Func<RefreshToken, bool>> condition)
        {
            return await GetAsQueryable(condition)
                .FirstOrDefaultAsync();
        }
    }
}
