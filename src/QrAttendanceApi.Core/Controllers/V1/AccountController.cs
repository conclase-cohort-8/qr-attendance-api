using Microsoft.AspNetCore.Mvc;

namespace QrAttendanceApi.Core.Controllers.V1
{
    [Route("api/v{version:apiversion}/account")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ApiBaseController
    {
        public AccountController()
        {
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This works");
        }
    }
}
