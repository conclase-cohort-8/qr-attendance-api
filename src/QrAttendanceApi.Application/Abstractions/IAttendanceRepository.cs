using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface IAttendanceRepository : IRepository<AttendanceLog>
    {
    }
}
