using Microsoft.EntityFrameworkCore;
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

        public async Task AddAsync(Department department, bool save = true)
        {
            await InsertAsync(department);
            if (save)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Department>> GetAll()
        {
            return await GetAsQueryable(d => !d.IsDeprecated)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }
    }
}
