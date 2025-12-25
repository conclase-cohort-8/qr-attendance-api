using QrAttendanceApi.Application.Responses;

namespace QrAttendanceApi.Core.Controllers.Extensions
{
    public static class BaseResponseExtensions
    {
        public static OkResponse<T> GetResult<T>(this ApiBaseResponse response)
        {
            return (OkResponse<T>)response;
        }
    }
}
