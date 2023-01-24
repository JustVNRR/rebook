namespace ReBook.ViewModels.AccountVM
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email_L { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password_L { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        public string Error { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
