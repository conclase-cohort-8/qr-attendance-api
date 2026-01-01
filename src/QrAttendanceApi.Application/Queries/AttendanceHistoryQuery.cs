using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Queries
{
    public class AttendanceHistoryQuery : PageQuery
    {
        public string? Search { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTime? Date { get; set; }
        public QrSessionType? Type { get; set; }
        public string? UserRole { get; set; }
    }
}