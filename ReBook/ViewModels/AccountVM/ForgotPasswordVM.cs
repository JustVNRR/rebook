namespace ReBook.ViewModels.AccountVM
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Error { get; set; }
        public string PageTitle { get; set; }

        public ForgotPasswordVM()
        {
            PageTitle = "Password Recovery";
            Error = null;
        }
    }
}
