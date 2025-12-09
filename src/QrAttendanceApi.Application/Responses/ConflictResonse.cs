namespace QrAttendanceApi.Application.Responses
{
    public class ConflictResonse : ApiBaseResponse
    {
        public ConflictResonse(string message)
            : base(false, 409, message)
        {
            
        }
    }
}
