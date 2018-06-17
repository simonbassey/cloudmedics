using System;
using System.Threading.Tasks;
using CloudMedics.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using System.Linq;

namespace CloudMedics.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _configuration;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailMessage message)
        {
            try
            {

                bool messageSent = false;
                var mailMessage = BuildEMailMessage(message);
                using (SmtpClient smtpClient = CreateSmtpclient())
                {
                    smtpClient.SendCompleted += (sender, eventArgs) =>
                    {
                        messageSent = (!eventArgs.Cancelled && eventArgs.Error == null);
                    };
                    await smtpClient.SendMailAsync(mailMessage);
                    return messageSent;
                }

            }
            catch (Exception exception)
            {
                _logger.LogError("Exception occured while sending email message -> {0}", exception);
                throw;
            }
        }
        private MailMessage BuildEMailMessage(EmailMessage email)
        {
            try
            {

                var message = new MailMessage
                {
                    To = { new MailAddress(email.To) },
                    From = new MailAddress(email.Sender, email.FromTitle),
                    Subject = email.Subject,
                    Body = email.Body,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    IsBodyHtml = email.IsHtml
                };
                email.BCC.ForEach(address => message.Bcc.Add(address));
                email.CC.ForEach(address => message.CC.Add(address));
                if (email.HasAttachment)
                    email.Attachments.ForEach(attachment => message.Attachments.Add(attachment));
                return message;

            }
            catch (Exception exception)
            {
                _logger.LogError("Exception was thrown Building EmailMessage -> {0}", exception);
                throw;
            }
        }
        private SmtpClient CreateSmtpclient()
        {
            var smptpSettings = ReadSmtpConfigSettings();
            return new SmtpClient
            {
                Port = int.Parse(smptpSettings.Item2),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                UseDefaultCredentials = true,
                Host = smptpSettings.Item1,
                Credentials = new NetworkCredential(smptpSettings.Item3, smptpSettings.Item4)
            };
        }
        private Tuple<string, string, string, string> ReadSmtpConfigSettings()
        {
            var settings = (
                smtp: _configuration["smtp:smtp"],
                port: _configuration["smtp:port"],
                username: _configuration["smtp:username"],
                password: _configuration["smtp:password"]
            );
            return Tuple.Create<string, string, string, string>(settings.smtp, settings.port, settings.username, settings.password);
        }
    }
}
