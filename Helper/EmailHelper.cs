using ManyRoomStudio.Models.Entities;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ManyRoomStudio.Helper
{
    public interface IEmailHelper
    {
        void SetCredentials(EmailSetupDetail setup);
        void To(string email);
        void Cc(string email);
        void Bcc(string email);
        void Subject(string subject);
        void Body(string body);
        void AddAttachment(Stream fileStream, string fileName, string mediaType);
        bool Send();
    }
    public class EmailHelper : IEmailHelper
    {
        private MailMessage _mailMessage;
        private SmtpClient _smtpClient;
        private EmailSetupDetail _setup;

        public EmailHelper()
        {
            _mailMessage = new MailMessage();
        }

        public void SetCredentials(EmailSetupDetail setup)
        {
            _setup = setup;

            _mailMessage.From = new MailAddress(setup.FromEmail);

            _smtpClient = new SmtpClient
            {
                Host = _setup.Host,
                Port = _setup.Port ?? 587,
                EnableSsl = _setup.EnableSsl ?? true,
                UseDefaultCredentials = _setup.UseDefaultCredentials ?? false,
                Credentials = new NetworkCredential(_setup.UserName, _setup.Password)
            };
        }

        public void To(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
                _mailMessage.To.Add(email);
        }

        public void Cc(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
                _mailMessage.CC.Add(email);
        }

        public void Bcc(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
                _mailMessage.Bcc.Add(email);
        }

        public void Subject(string subject)
        {
            _mailMessage.Subject = subject;
        }

        public void Body(string body)
        {
            _mailMessage.Body = body;
            _mailMessage.IsBodyHtml = true;
        }

        public void AddAttachment(Stream fileStream, string fileName, string mediaType)
        {
            if (fileStream != null && !string.IsNullOrWhiteSpace(fileName))
            {
                var attachment = new Attachment(fileStream, fileName, mediaType ?? MediaTypeNames.Application.Octet);
                _mailMessage.Attachments.Add(attachment);
            }
        }

        public bool Send()
        {
            try
            {
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _mailMessage.Dispose();
                _smtpClient.Dispose();
            }
        }
    }
}
