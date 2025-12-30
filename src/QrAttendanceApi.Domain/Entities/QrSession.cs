using QrAttendanceApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QrAttendanceApi.Domain.Entities
{
    public class QrSession : BaseEntity
    {
        [Required, StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime LateAfter { get; set; }
        [Required]
        public DateTime EndsAt { get; set; }
        [Range(60, int.MaxValue)]
        public int RegenerateTokenInSeconds { get; set; }
        public bool IsActive { get; set; } = true;
        [Required]
        [EnumDataType(typeof(QrSessionType))]
        [Column(TypeName = "varchar(30)")]
        public QrSessionType Type { get; set; }

        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public User? CreatedBy { get; set; }
    }
}
