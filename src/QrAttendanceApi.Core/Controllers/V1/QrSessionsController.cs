using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Services.Abstractions;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/qr-sessions")]
    [ApiVersion("1.0")]
    [ApiController]
    public class QrSessionsController : ApiBaseController
    {
        private readonly IServiceManager _manager;

        public QrSessionsController(IServiceManager manager)
        {
            _manager = manager;
        }
    }
}