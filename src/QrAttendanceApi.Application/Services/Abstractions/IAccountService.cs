using QrAttendanceApi.Application.Commands.Accounts;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IAccountService
    {
        Task<ApiBaseResponse> LoginAsync(LoginCommand command);
        Task<ApiBaseResponse> RefreshAsync(RefreshTokenCommand command);
        Task<ApiBaseResponse> RegisterAsync(RegisterCommand command);
    }
}
