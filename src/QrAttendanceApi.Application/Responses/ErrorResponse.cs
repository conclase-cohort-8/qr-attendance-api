namespace QrAttendanceApi.Application.Responses
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int MyProperty { get; set; }
        public int? Data { get; set; }
    }
}
