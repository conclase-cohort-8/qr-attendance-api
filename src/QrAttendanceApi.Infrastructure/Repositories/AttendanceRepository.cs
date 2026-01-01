using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class AttendanceRepository : Repository<AttendanceLog>, IAttendanceRepository
    {
        public AttendanceRepository(AppDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
