using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;
using System.Text.Json.Serialization;

namespace QrAttendanceApi.Application.Commands.Accounts
{
    public class RegisterCommand
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ComfirmPassword { get; set; } = string.Empty;
        [JsonIgnore]
        public Roles Role { get; set; } = Roles.Student;
        public Guid DepartmentId { get; set; }

        public static User MapUser(RegisterCommand command)
        {
            return new User
            {
                FullName = command.FullName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                UserName = command.Email,
                IsActive = true,
                EmailConfirmed = true,
                DepartmentId = command.DepartmentId
            };
        }
    }
}
