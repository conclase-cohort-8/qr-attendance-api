using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using QrAttendanceApi.Application.Commands.QRs;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Core.Controllers.Extensions;
using QrAttendanceApi.Infrastructure.ExternalServices;

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



        [HttpPost]
        [Authorize(Roles = "Staff,Admin,SuperAdmin")]
        public async Task<IActionResult> Post([FromBody] CreateQrSessionCommand command)
        {
            var loggedInUserId = HttpContext.GetLoggedInUserId();
            var result = await _manager.QrSession.Create(loggedInUserId, command);
            if (!result.Success)
            {
                return ProcessError(result);
            }
            return Ok(result.GetResult<CreateQrResponseDto>);

        }

        [HttpPost("{id}/token")]
        [Authorize(Roles = "Staff,Admin,SuperAdmin")]

        public async Task<IActionResult> GenerateQr([FromRoute] Guid id)
        {
            var loggedInUserId = HttpContext.GetLoggedInUserId();
            var result = await _manager.QrSession.GenerateQrToken(loggedInUserId, id);
            if (!result.Success)
            {
                return ProcessError(result);
            }
            return Ok(result.GetResult<QrTokenDto>());


        }

        [HttpPost("{id}/token-Image")]
        [Authorize(Roles = "Staff,Admin,SuperAdmin")]

        public async Task<IActionResult> GenerateQrPng([FromRoute] Guid id)
        {
            var loggedInUserId = HttpContext.GetLoggedInUserId();
            var result = await _manager.QrSession.GenerateQrToken(loggedInUserId, id);
            if (!result.Success)
            {
                return ProcessError(result);
            }
            var token = result.GetResult<QrTokenDto>();
            var qrByts = QrCodeHelper.Generate(token.Data!.Token);
            return File(qrByts,"Image/png");


        }


        [HttpPost("{id}/validate")]
        [Authorize]

        public async Task<IActionResult> ValidateQr([FromRoute] Guid id, SessionAttendanceCommand command)
        {
            var loggedInUserId = HttpContext.GetLoggedInUserId();
            var result = await _manager.Attendance.MarkAttendance(loggedInUserId, id, command);
            if (!result.Success)
            {
                return ProcessError(result);
            }
            return Ok(result.GetResult<QrTokenDto>());


        }
    }

}