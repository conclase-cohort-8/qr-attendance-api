namespace QrAttendanceApi.Application.Responses
{
    public class UnauthorizedResponse : ApiBaseResponse
    {
        public UnauthorizedResponse(string message) : 
            base(false, 401, message)
        { }
    }
}
