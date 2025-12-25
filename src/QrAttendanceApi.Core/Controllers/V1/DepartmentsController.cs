using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Commands.Departments;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Core.Controllers.Extensions;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/departments")]
    [ApiVersion("1.0")]
    [ApiController]
    public class DepartmentsController : ApiBaseController
    {
        private readonly IServiceManager _service;

        public DepartmentsController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a department
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(AddDepartmentCommand command)
        {
            var response = await _service.Department
                .CreateAsync(command);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<string>());
        }

        /// <summary>
        /// Gets a list of departments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Department.GetDepartments();
            return Ok(response.GetResult<List<DepartmentDto>>());
        }
    }
}
