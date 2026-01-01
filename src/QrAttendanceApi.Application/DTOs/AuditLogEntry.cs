using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.DTOs
{
    public record AuditLogEntry(ActionType Action, 
        Guid EntityId, 
        string UserId, 
        string EntityType,
        string? Reason = null);
}
