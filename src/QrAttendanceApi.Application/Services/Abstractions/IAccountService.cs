using Microsoft.AspNetCore.Http;
using QrAttendanceApi.Application.Commands.Accounts;
using QrAttendanceApi.Application.Commands.Users;
using QrAttendanceApi.Application.Queries;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Application.Services.Abstractions
{
    public interface IAccountService
    {
        Task<ApiBaseResponse> LoadUserDataAsync(IFormFile file);
        Task<ApiBaseResponse> LoginAsync(LoginCommand command);
        Task<ApiBaseResponse> RefreshAsync(RefreshTokenCommand command);
        Task<ApiBaseResponse> RegisterAsync(RegisterCommand command);
        Task<ApiBaseResponse> UpdateDetailsAsync(string? userId, UserUpdateCommand command);
        Task<ApiBaseResponse> DeactivateAsync(string userId);
        Task<ApiBaseResponse> ActivateAsync(string userId);
        Task<ApiBaseResponse> GetPagedUsers(UserPagedQuery query);
    }
}
