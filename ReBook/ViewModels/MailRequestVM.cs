namespace ReBook.ViewModels
{
    public class MailRequestVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
