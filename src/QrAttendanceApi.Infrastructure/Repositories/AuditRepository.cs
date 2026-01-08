using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _dbContext;

        public AuditRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            await _dbContext.Set<AuditLog>().AddAsync(auditLog);
        }
    }
}