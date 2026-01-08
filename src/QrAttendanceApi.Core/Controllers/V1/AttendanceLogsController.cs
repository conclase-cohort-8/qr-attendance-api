using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Commands.Attendance;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Core.Controllers.Extensions;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/account")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AttendanceLogsController : ApiBaseController
    {
        private readonly IServiceManager _service;

        public AttendanceLogsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAttendanceByadmin([FromBody] CreateAttendanceAdminCommand command)
        {
            var adminId = HttpContext.GetLoggedInUserId();
            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Forbid();
            }

            var response = await _service.Attendance.CreateAdmin(adminId, command);

            return StatusCode(response.Status, response);
        }

        [HttpPut("{attendanceId:guid}/admin")]
        public async Task<IActionResult> UpdateAttendance(Guid attendanceId, [FromBody] 
        EditAttendanceCommand command)
        {
            var adminId = HttpContext.GetLoggedInUserId();
            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Forbid();
            }

            var response = await _service.Attendance.Edit(adminId, attendanceId, command);

            return StatusCode(response.Status, response);
        }

        [Http("{attendanceId:guid}/admin")]
        public async Task<IActionResult> DeleteAttendance(Guid attendanceId, [FromBody]
        DeleteAttendanceCommand command)
        {
            var adminId = HttpContext.GetLoggedInUserId();
            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Forbid();
            }

            var response = await _service.Attendance.Delete(adminId, attendanceId, command);

            return StatusCode(response.Status, response);
        }

    }
}
