using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Commands.QRs
{
    public class CreateQrSessionCommand
    {
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public int DurationMinutes { get; set; }
        public int LateAfterMinutes { get; set; }
        public QrSessionType Type { get; set; }
        public string Description { get; set; } = default!;
        public Guid DepartmentId { get; set; }
    }
}