using Microsoft.AspNetCore.Identity;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepositoryManager _repository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AttendanceService(IRepositoryManager repository,
            ITokenService tokenService, UserManager<User> userManager)
        {
            _repository = repository;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<ApiBaseResponse> MarkAttendance(string? userId, Guid sessionId, SessionAttendanceCommand command)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ForbiddenResponse("Access denied");
            }
            if (string.IsNullOrWhiteSpace(command.QrToken))
            {
                return new ForbiddenResponse("Access denied");
            }

            var tokenIsValid = _tokenService.TryValidateQrToken(command.QrToken, sessionId, out var reason);
            if (!tokenIsValid)
            {
                return new ForbiddenResponse(reason);
            }

            var loggedInUser = await _userManager.FindByIdAsync(userId);
            if (loggedInUser == null)
            {
                return new NotFoundResponse("User not found");
            }

            var session = await _repository.QrSession
                .FirstOrDefault(s => s.Id == sessionId && s.IsActive && s.EndsAt > DateTime.UtcNow);
            if (session == null)
            {
                return new NotFoundResponse("Session not found");
            }

            var existingAttendance = await _repository.Attendance.ExistsAsync(a => a.UserId == loggedInUser.Id &&
                a.QrSessionId == session.Id);
            if (existingAttendance)
            {
                return new ConflictResponse("Already recorded attendance");
            }

            var attendance = new AttendanceLog
            {
                UserId = loggedInUser.Id,
                QrSessionId = session.Id,
                Status = DateTime.UtcNow > session.LateAfter ? AttendanceStatus.Late : AttendanceStatus.Present
            };
            await _repository.Attendance.AddAsync(attendance);
            await _repository.SaveAsync();

            return new OkResponse<string>("Attendance recorded successfully");
        }
    }
}
