using QrAttendanceApi.Application.DTOs;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface IAuditEntry
    {
        Task WriteAsync(AuditLogEntry entry);
    }
}
