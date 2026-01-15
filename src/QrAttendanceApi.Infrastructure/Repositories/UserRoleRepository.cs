using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRoleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<(string UserId, string? RoleName)>> GetUserRoles(List<string> userIds)
        {
            var userRoles = await _dbContext.UserRoles
                .Where(ur => userIds.Contains(ur.UserId))
                .Join(_dbContext.Roles, 
                        ur => ur.RoleId, 
                        r => r.Id, 
                        (ur, r) => new { ur.UserId, r.Name })
                .ToListAsync();

            return userRoles.Select(ur => (ur.UserId, ur.Name)).ToList();
        }
    }
}