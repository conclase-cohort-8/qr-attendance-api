using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Commands.QRs
{
    public class CreateQrSessionCommand
    {
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public int DurationMinutes { get; set; }
        public int LateAfterMinutes { get; set; }
        public QrSessionType Type { get; set; }
        public string Description { get; set; } = default!;
        public Guid DepartmentId { get; set; }

        public static QrSession ToSessionModel(string userId, CreateQrSessionCommand command)
        {
            return new QrSession
            {
                Description = command.Description,
                CreatedById = userId,
                StartedAt = command.StartTime,
                LateAfter = command.StartTime.AddMinutes(command.LateAfterMinutes),
                EndsAt = command.StartTime.AddMinutes(command.DurationMinutes),
                DepartmentId = command.DepartmentId,
                Type = command.Type
            };
        }
    }
}