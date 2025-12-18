using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Domain.Entities
{
    public class QrSession : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public string Nonce { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsDynamic { get; set; }
        public bool IsActive { get; set; }
        public Guid? StackId { get; set; }
        public QrSessionType Type { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public User? CreatedBy { get; set; }
    }
}
