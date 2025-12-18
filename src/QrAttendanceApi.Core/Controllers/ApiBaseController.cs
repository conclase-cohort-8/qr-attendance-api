using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Core.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ProcessError(ApiBaseResponse response)
        {
            return StatusCode(response.Status, response);
        }
    }
}
