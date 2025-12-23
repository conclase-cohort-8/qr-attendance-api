namespace QrAttendanceApi.Application.Responses
{
    public class ConflictResponse : ApiBaseResponse
    {
        public ConflictResponse(string message)
            : base(false, 409, message)
        {
            
        }
    }
}
