using System.ComponentModel.DataAnnotations;

namespace QrAttendanceApi.Application.Commands.Attendance
{
    public class DeleteAttendanceCommand
    {
        [Required]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public Guid AttendanceId { get; set; }
    }
}
