using SyllabusManager.App.Interfaces;
using SyllabusManager.App.Services.External;

namespace SyllabusManager.App.Services
{
    // ADAPTER: Adapts ThirdPartyEmailService to INotificationChannel
    public class EmailAdapter : INotificationChannel
    {
        private readonly ThirdPartyEmailService _externalService;

        public EmailAdapter()
        {
            _externalService = new ThirdPartyEmailService();
        }

        public void Send(string recipient, string message)
        {
            // Adapting the call
            // We interpret "recipient" as email address and "message" as body
            // We generate a generic subject
            string subject = "Syllabus Manager Notification";
            _externalService.SendEmail(recipient, subject, message);
        }
    }
}
