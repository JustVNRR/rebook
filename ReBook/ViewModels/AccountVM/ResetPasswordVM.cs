namespace ReBook.ViewModels.AccountVM
{
    public class ResetPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
        public string PageTitle { get; set; }

        public ResetPasswordVM()
        {
            PageTitle = "Reset Password";
            Error = null;
        }
    }
}
