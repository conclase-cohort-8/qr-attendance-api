using System.ComponentModel.DataAnnotations;

namespace QrAttendanceApi.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        [Required]
        public string Hash { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
