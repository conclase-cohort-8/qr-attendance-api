using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Domain.Entities
{
    public class AttendanceLog : BaseEntity
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public decimal? LocationLat { get; set; }
        public decimal? LocationLng { get; set; }
        public string? DeviceInfo { get; set; }
        public AttendanceStatus Status { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        public Guid QrSessionId { get; set; }
        public QrSession? QrSession { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}