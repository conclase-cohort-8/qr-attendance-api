using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Core.Controllers.Extensions;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/analytics")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize (Roles = "Admin,SuperAdmin")]
    public class AnalyticsController : ApiBaseController
    {
        private readonly IServiceManager _service;

        public AnalyticsController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Dashboard summary analytics
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>       
        [HttpGet("dashboard/summary")]
        public async Task<IActionResult> GetDashboardSummary([FromQuery] DateTime date)
        {
            var response = await _service.Analytics.GetDashboardSummaryAsync(date);

            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<DashboardSummaryDto>());
        }

        /// <summary>
        /// Dashboard department analytics
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>    
        [HttpPost("dashboard/departments")]
        public async Task<IActionResult> GetDepartmentBreakdown([FromQuery] DateTime date)
        {
            var response = await _service.Analytics.GetDepartmentBreakdownAsync(date);

            if (!response.Success)
                return ProcessError(response);

            return Ok (response.GetResult<List<DepartmentAttendanceBreakdownDto>>());
        }

        /// <summary>
        /// Attendance trends analytics
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>    
        [HttpPost("trends")]
        public async Task <IActionResult> GetAttendanceTrends([FromQuery] DateTime from , [FromQuery] DateTime to)
        {
            var response = await _service.Analytics.GetAttendanceTrendsAsync(from, to);

            if (!response.Success)
                return ProcessError(response);

            return Ok(response.GetResult<List<AttendanceTrendDto>>());
        }

    }
}
