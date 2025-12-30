using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.DTOs;

namespace QrAttendanceApi.Application.Services
{
    public class AuditLogJob
    {
        private readonly IAuditEntry _entry;

        public AuditLogJob(IAuditEntry entry)
        {
            _entry = entry;
        }

        public async Task Execute(AuditLogEntry entry)
        {
            await _entry.WriteAsync(entry);
        }
    }
}
