namespace ReBook.ViewModels.AccountVM
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Remote(action: "IsUserPseudoInUse", controller: "Account")]
        public string Pseudo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Avatar { get; set; }
        public string Error { get; set; }
        public RegisterVM()
        {
            Avatar = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624735508/Avatars/anonymous_placeholder_szrplp.jpg";
        }
    }
}
