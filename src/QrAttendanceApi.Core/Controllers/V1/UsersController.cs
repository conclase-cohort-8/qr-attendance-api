using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Commands.Users;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Queries;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Core.Controllers.Extensions;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ApiBaseController
    {
        private readonly IServiceManager _manager;

        public UsersController(IServiceManager manager)
        {
            _manager = manager;
        }
        
        /// <summary>
        /// Gets a paginated list of users
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Get([FromQuery] UserPagedQuery query)
        {
            var response = await _manager.Account.GetPagedUsers(query);
            return Ok(response.GetResult<PagedResponse<UserDto>>());
        }

        /// <summary>
        /// Updates users' basic information
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> PatchUser([FromBody] UserUpdateCommand command)
        {
            var userId = HttpContext.GetLoggedInUserId();
            var response = await _manager.Account.UpdateDetailsAsync(userId, command);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<string>());
        }
    }
}