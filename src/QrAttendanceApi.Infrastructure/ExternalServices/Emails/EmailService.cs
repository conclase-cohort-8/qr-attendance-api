using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Settings;

namespace QrAttendanceApi.Infrastructure.ExternalServices.Emails
{
    public class EmailService : IEmailService
    {
        private readonly MailJetSettings _mailJetSettings;
        private readonly IMailjetClient _mailjetClient;
        public EmailService(IOptions<MailJetSettings> settings, IMailjetClient mailjetClient)
        {
            _mailJetSettings = settings.Value;
            _mailjetClient = mailjetClient;
        }

        public async Task<bool> SendAsync(string recipient, string message, string subject)
        {
            try {

                var request = new MailjetRequest
                {
                    Resource =  SendV31.Resource
                }.Property("Messages" ,new JArray
                {
                    new JObject {

                        {
                            "From",
                            new JObject
                            {
                                { "Email", _mailJetSettings.Email },
                                { "Name", _mailJetSettings.AppName }
                            }
                        },
                        {
                            "To",
                            new JArray
                            {
                                new JObject
                                {
                                    { "Email", recipient }
                                }
                            }
                        },
                        { "Subject", subject },
                        { "TextPart", message },
                        { "HtmlPart", message }

                    }
                });
                var response = await _mailjetClient.PostAsync(request);
                return response?.IsSuccessStatusCode ?? false;

            }
            catch (Exception ex) {

                throw;

            }
        }
    }
}
