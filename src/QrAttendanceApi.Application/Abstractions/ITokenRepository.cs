using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Abstractions
{
    public interface ITokenRepository : IRepository<RefreshToken>
    {
    }
}
