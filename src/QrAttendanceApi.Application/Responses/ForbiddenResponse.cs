namespace QrAttendanceApi.Application.Responses
{
    public class ForbiddenResponse : ApiBaseResponse
    {
        public ForbiddenResponse(string message)
            : base(false, 403, message)
        {
            
        }
    }
}
