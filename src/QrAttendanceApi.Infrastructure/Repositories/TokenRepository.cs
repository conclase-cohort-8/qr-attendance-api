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
    }
}
