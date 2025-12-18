using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Commands.Accounts;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Core.Controllers.Extensions;
using QrAttendanceApi.Domain.Entities;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/account")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ApiBaseController
    {
        private readonly IServiceManager _service;

        public AccountController(IServiceManager service)
        {
            _service = service;
        }

       /// <summary>
       /// Logs in a user
       /// </summary>
       /// <param name="command"></param>
       /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDto), 200)]
        public async Task<IActionResult> LoginAsync(LoginCommand command)
        {
            var result = await _service.Account.LoginAsync(command);
            if (!result.Success)
            {
                return ProcessError(result);
            }

            return Ok(result.GetResult<TokenDto>());
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> RegisterAsync(RegisterCommand command)
        {
            var result = await _service.Account.RegisterAsync(command);
            if (!result.Success)
            {
                return ProcessError(result);
            }

            return Ok(result.GetResult<string>());
        }

        /// <summary>
        /// Generate a new access token using the refresh token
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("refresh")]
        [ProducesResponseType(typeof(TokenDto), 200)]
        public async Task<IActionResult> RefreshAsync(RefreshTokenCommand command)
        {
            var response = await _service.Account.RefreshAsync(command);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<TokenDto>());
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, SuperAdmin")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> DataLoad(IFormFile file)
        {
            var response = await _service.Account.LoadUserDataAsync(file);
            if (!response.Success)
            {
                return ProcessError(response);
            }

            return Ok(response.GetResult<List<User>>());
        }
    }
}
