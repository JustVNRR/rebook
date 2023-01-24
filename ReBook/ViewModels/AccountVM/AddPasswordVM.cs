namespace ReBook.ViewModels.AccountVM
{
    public class AddPasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Error { get; set; }

        public AddPasswordVM()
        {
            Error = null;
        }
    }
}
