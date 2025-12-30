using Hangfire;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Services
{
    public static class AuditLogger
    {
        public static void Log(ActionType action, 
                                string userId, 
                                Guid entityId, 
                                string entity,
                                string? description)
        {
            var entry = new AuditLogEntry(
                action, 
                entityId, 
                userId, 
                entity,
                description);

            BackgroundJob.Enqueue<AuditLogJob>(
                job => job.Execute(entry)
            );
        }
    }

}