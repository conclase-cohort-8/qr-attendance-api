using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IAttendanceService
    {
        Task<ApiBaseResponse> MarkAttendance(string userId, SessionAttendanceCommand command);
    }
}
