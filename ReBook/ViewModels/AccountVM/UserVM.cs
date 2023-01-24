namespace ReBook.ViewModels.AccountVM
{
    public class UserVM
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Remote("IsEmailInUse", "Account", AdditionalFields = "Id")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Remote("IsUserPseudoInUse", "Account", AdditionalFields = "Id")]
        public string Pseudo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Avatar { get; set; }
        public List<string> Roles { get; set; }
        public string Error { get; set; }
        public string PageTitle { get; set; }

        public UserVM()
        {
            Id = null;
            PageTitle = "New User";
            Error = null;
            Roles = new List<string>();

            Avatar = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624735508/Avatars/anonymous_placeholder_szrplp.jpg";
        }
    }
}
