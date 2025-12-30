using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;

namespace QrAttendanceApi.Application.Services
{
    public class QrSessionService : IQrSessionService
    {
        private readonly IRepositoryManager _manager;

        public QrSessionService(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Task<ApiBaseResponse> Create(string userId, CreateQrSessionCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
