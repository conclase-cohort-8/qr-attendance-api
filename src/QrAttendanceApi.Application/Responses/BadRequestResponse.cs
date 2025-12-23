namespace QrAttendanceApi.Application.Responses
{
    public class BadRequestResponse : ApiBaseResponse
    {
        public BadRequestResponse(string message) 
            : base(false, 400, message)
        {

        }
    }
}
