using QrAttendanceApi.Application.Abstractions.Externals;

namespace QrAttendanceApi.Infrastructure.ExternalServices.Emails
{
    public class EmailService : IEmailService
    {
        public Task<bool> SendAsync(string recipient, string message, string subject)
        {
            throw new NotImplementedException();
        }
    }
}
