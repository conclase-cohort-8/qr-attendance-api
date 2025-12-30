using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QrAttendanceApi.Domain.Entities
{
    public class User : IdentityUser
    {
        [Required, StringLength(200)]
        public string FullName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }
        public bool IsActive { get; set; }

        public string? ExternalId { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

    }
}
