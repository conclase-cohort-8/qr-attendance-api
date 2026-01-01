using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IQrSessionService
    {
        Task<ApiBaseResponse> Create(string? userId, CreateQrSessionCommand command);
        Task<ApiBaseResponse> GenerateQrToken(string? userId, Guid sessionId);
    }
}
