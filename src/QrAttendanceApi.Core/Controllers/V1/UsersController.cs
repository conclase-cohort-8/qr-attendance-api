using Microsoft.AspNetCore.Mvc;
using QrAttendanceApi.Application.Services.Abstractions;

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
    }
}