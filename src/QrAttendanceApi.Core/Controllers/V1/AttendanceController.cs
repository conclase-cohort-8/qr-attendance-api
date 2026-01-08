using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Queries;
using QrAttendanceApi.Application.Services.Abstractions;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/attendance")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AttendanceController : ApiBaseController
    {
        private readonly IServiceManager _service;

        public AttendanceController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetAttendanceHistory([FromQuery] AttendanceHistoryQuery query)
        {
            var response = await _service.Attendance.GetHistory(query);

            return StatusCode(response.Status, response);
        }
    }
}
