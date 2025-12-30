using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;

namespace QrAttendanceApi.Infrastructure.Repositories
{
    public class AuditEntry : IAuditEntry
    {
        private readonly AppDbContext _context;

        public AuditEntry(AppDbContext context)
        {
            _context = context;
        }

        public async Task WriteAsync(AuditLogEntry entry)
        {
            await _context.AuditLogs.AddAsync(new AuditLog
            {
                Action = entry.Action,
                EntityId = entry.EntityId,
                UserId = entry.UserId,
                EntityType = entry.EntityType,
                Description = entry.Reason
            });

            await _context.SaveChangesAsync();
        }
    }
}