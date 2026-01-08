using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Commands.Attendance
{
    public class CreateAttendanceAdminCommand
    {
        public string UserId { get; set; } = string.Empty;
        public Guid QrSessionId { get; set; }
        public DateTime Timestamp { get; set; }
        public AttendanceStatus Status { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
