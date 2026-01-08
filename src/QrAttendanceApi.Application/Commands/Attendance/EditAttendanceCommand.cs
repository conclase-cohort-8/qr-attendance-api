using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Commands.Attendance
{
    public class EditAttendanceCommand
    {
        public DateTime Timestamp { get; set; }
        public AttendanceStatus Status { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
