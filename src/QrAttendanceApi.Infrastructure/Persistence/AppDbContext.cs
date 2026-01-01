using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Configurations;

namespace QrAttendanceApi.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<QrSession> QrSessions { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : 
            base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("smart_attendance");

            builder.ApplyConfiguration(new RolesConfigurations());
        }
    }
}
