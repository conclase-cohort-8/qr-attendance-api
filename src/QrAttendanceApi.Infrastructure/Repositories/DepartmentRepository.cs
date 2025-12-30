using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly AppDbContext dbContext;

        public DepartmentRepository(AppDbContext dbContext) :
            base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
