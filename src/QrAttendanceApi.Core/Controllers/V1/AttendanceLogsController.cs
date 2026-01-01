using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Services.Abstractions;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/account")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AttendanceLogsController : ApiBaseController
    {
        private readonly IServiceManager _service;

        public AttendanceLogsController(IServiceManager service)
        {
            _service = service;
        }
    }
}
