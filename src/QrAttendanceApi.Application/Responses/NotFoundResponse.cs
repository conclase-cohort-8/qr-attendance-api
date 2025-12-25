namespace QrAttendanceApi.Application.Responses
{
    public class NotFoundResponse : ApiBaseResponse
    {
        public NotFoundResponse(string message)
            : base(false, 404, message)
        {
            
        }
    }
}
