using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ReBook.Settings;
using System.IO;

namespace ReBook.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequestVM mailRequest)
        {
            var email = new MimeMessage();
            if (mailRequest.FromEmail == null)
            {
                email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            }
            else
            {
                email.From.Add(new MailboxAddress(mailRequest.FromEmail, _mailSettings.Mail));
                 email.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
                //email.To.Add(MailboxAddress.Parse("lionelbos@gmail.com"));
            }

            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Timeout = 4000;
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailConfirmationAsync(string userMail, string confirmationLink)
        {
            MailRequestVM mailRequest = new()
            {
                // ToEmail = userMail,
                ToEmail = _mailSettings.Mail,
                Subject = "Please confirm your inscription to Rebook",
                Body = "<p>This email address was used to register on Rebook.com." +
                                    "Please ignore this message if you are not the initiator of this action." +
                                    $"Otherwise click <a href='{confirmationLink}'>here</a> to confirm your registration.</p>"
            };
            await SendEmailAsync(mailRequest);
        }

        public async Task SendResetPasswordAsync(string userMail, string passwordResetLink)
        {
            MailRequestVM mailRequest = new()
            {
                // ToEmail = userMail,
                ToEmail = _mailSettings.Mail,
                Subject = "Reset your password on Rebook",
                Body = "<p>This email address was used to reset password account on Rebook.com." +
                                    "Please ignore this message if you are not the initiator of this action." +
                                    $"Otherwise click <a href='{passwordResetLink}'>here</a> to reste your password.</p>"
            };
            await SendEmailAsync(mailRequest);
        }
    }
}
