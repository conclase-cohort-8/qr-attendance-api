using System.ComponentModel.DataAnnotations;

namespace QrAttendanceApi.Domain.Entities
{
    public class Department : BaseEntity
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<User> Users { get; set; } = [];
    }
}
