using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;

namespace QrAttendanceApi.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepositoryManager _repository;
        private readonly ITokenService _tokenService;

        public AttendanceService(IRepositoryManager repository,
            ITokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public Task<ApiBaseResponse> MarkAttendance(string userId, SessionAttendanceCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
