namespace ReBook.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequestVM mailRequest);
        Task SendEmailConfirmationAsync(string userMail, string confirmationLink);
        Task SendResetPasswordAsync(string userMail, string passwordResetLink);
    }
}
