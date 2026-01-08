using QrAttendanceApi.Application.Commands.Attendance;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Queries;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IAttendanceService
    {
        Task<ApiBaseResponse> MarkAttendance(string? userId, Guid sessionId, SessionAttendanceCommand command);

        Task<ApiBaseResponse> GetHistory(AttendanceHistoryQuery query);
        Task<ApiBaseResponse> CreateAdmin(string adminId, CreateAttendanceAdminCommand command);

        Task<ApiBaseResponse> Edit(string adminId, Guid attendanceId, EditAttendanceCommand command);

        Task<ApiBaseResponse> Delete(string adminId, Guid attendanceId, DeleteAttendanceCommand command);


    }

}