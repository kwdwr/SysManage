using System;

namespace SyllabusManager.App.Services.External
{
    // ADAPTEE: A mock 3rd party email service with a different API
    public class ThirdPartyEmailService
    {
        public void SendEmail(string toAddress, string subject, string body)
        {
            Console.WriteLine($"[ThirdPartyEmail] Connecting to SMTP server...");
            Console.WriteLine($"[ThirdPartyEmail] Sending to {toAddress} | Subject: {subject}");
            Console.WriteLine($"[ThirdPartyEmail] Body: {body}");
            Console.WriteLine($"[ThirdPartyEmail] Sent successfully.");
        }
    }
}
