using Microsoft.Extensions.Options;
using ReStudyAPI.Models.Common;
using System.Net.Mail;

namespace ReStudyAPI.Utility.Helpers
{
    public class EmailUtility : IEmailUtility
    {
        private readonly IOptions<EmailConfiguration> _emailConfiguration;
        private SmtpClient _smtpClient;
        public EmailUtility(SmtpClient smtpClient, IOptions<EmailConfiguration> config)
        {
            _emailConfiguration = config;
            _smtpClient = smtpClient;
        }

        public async Task SendEmailAsync(string receiverEmail, string subject, string body)
        {
            if (_emailConfiguration == null || _emailConfiguration?.Value == null)
            {
                return;
            }
            var emailConfig = _emailConfiguration.Value;

            MailMessage mail = new MailMessage()
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.From = new MailAddress(emailConfig.SenderEmailAddress, emailConfig.SenderName);
            mail.To.Add(receiverEmail);
            await _smtpClient.SendMailAsync(mail).ConfigureAwait(false);
        }

        public async Task<bool> SendBulkEmailAsync(List<string> emailAddressList, string subject, string body)
        {
            if (_emailConfiguration == null || _emailConfiguration.Value == null)
            {
                return false;
            }
            var emailConfig = _emailConfiguration.Value;
            string emailAddressBulk = string.Join(",", emailAddressList);
            MailMessage mail = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body,
            };
            mail.From = new MailAddress(emailConfig.SenderEmailAddress, emailConfig.SenderName);
            mail.To.Add(emailAddressBulk);
            await _smtpClient.SendMailAsync(mail).ConfigureAwait(false);
            return true;
        }
    }

    public interface IEmailUtility
    {
        Task SendEmailAsync(string receiverEmail, string subject, string body);
        Task<bool> SendBulkEmailAsync(List<string> emailAddressList, string subject, string body);
    }
}
