using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
