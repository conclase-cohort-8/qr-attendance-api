namespace QrAttendanceApi.Application.Responses
{
    public class ApiBaseResponse
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public ApiBaseResponse(bool success, int status, string message = "")
        {
            Status = status;
            Success = success;
            Message = message;
        }
    }
}