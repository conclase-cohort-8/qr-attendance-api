using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditLog auditLog);
    }
}