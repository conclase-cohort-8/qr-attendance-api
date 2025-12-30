using QrAttendanceApi.Application.Responses;
using System.Security.Claims;

namespace QrAttendanceApi.Core.Controllers.Extensions
{
    public static class BaseResponseExtensions
    {
        public static OkResponse<T> GetResult<T>(this ApiBaseResponse response)
        {
            return (OkResponse<T>)response;
        }

        public static string? GetLoggedInUserId(this HttpContext context)
        {
            return context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
