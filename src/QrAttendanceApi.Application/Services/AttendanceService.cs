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

        public Task<ApiBaseResponse> MarkAttendance(string? userId, Guid sessionId, SessionAttendanceCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
