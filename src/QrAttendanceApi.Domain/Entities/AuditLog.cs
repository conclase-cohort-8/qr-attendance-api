using QrAttendanceApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QrAttendanceApi.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string UserId { get; set; } = default!;
        [Required]
        public Guid EntityId { get; set; }
        [Required]
        [EnumDataType(typeof(ActionType))]
        [Column(TypeName = "varchar(50)")]
        public ActionType Action { get; set; }
        public string EntityType { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
    }
}