using Microsoft.AspNetCore.Identity;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Application.Services
{
    public class QrSessionService : IQrSessionService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public QrSessionService(IRepositoryManager manager,
                                UserManager<User> userManager,
                                ITokenService tokenService)
        {
            _repository = manager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public Task<ApiBaseResponse> Create(string? userId, CreateQrSessionCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<ApiBaseResponse> GenerateQrToken(string? userId, Guid sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
