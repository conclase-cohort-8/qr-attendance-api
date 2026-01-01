namespace QrAttendanceApi.Application.Abstractions.Externals
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string recipient, string message, string subject);
    }
}
