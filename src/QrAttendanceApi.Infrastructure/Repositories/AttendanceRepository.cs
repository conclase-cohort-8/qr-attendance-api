using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class AttendanceRepository : Repository<AttendanceLog>, IAttendanceRepository
    {
        private readonly AppDbContext _dbContext;
        public AttendanceRepository(AppDbContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> HasUserScannedAsync(Guid sessionId, Guid userId)
        {
            return await _dbContext.AttendanceLogs
               .AnyAsync(a =>
            a.QrSessionId == sessionId &&
            a.UserId == userId.ToString() &&
            !a.IsDeleted);
        }

        public async Task<List<AttendanceLog>> GetBySessionIdAsync(Guid sessionId)
        {
            return await _dbContext.AttendanceLogs
                .Where(a => a.QrSessionId == sessionId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task SoftDeleteAsync(AttendanceLog attendanceLog)
        {
            attendanceLog.IsDeleted = true;
            _dbContext.AttendanceLogs.Update(attendanceLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(List<AttendanceLog> Data, int TotalCount)> GetHistoryAsync(
            int page, int pageSize, Guid? departmentId = null, string? search = null, DateTime? from = null, DateTime? to = null)
        {
            var query = _dbContext.AttendanceLogs
                .Include(a => a.User)
                .Include(a => a.QrSession)
                .ThenInclude(s => s.Department)
                .Where(a => !a.IsDeleted)
                .AsQueryable();

            if (departmentId.HasValue)
                query = query.Where(a => a.QrSession.DepartmentId == departmentId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a => a.User.FullName.Contains(search));

            if (from.HasValue)
                query = query.Where(a => a.Timestamp >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.Timestamp <= to.Value);

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }
    }
}
