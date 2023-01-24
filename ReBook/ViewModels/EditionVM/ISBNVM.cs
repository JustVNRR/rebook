namespace ReBook.ViewModels.EditionVM
{
    public class ISBNVM
    {
        [Required(ErrorMessage = "Please enter 13 or 10 digits")]
        [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$",
         ErrorMessage = "Please enter 13 or 10 digits")]
        public string ISBN { get; set; }

        public string Repository { get; set; }
        public string CurrentController { get; set; }
        public ISBNVM()
        {
            Repository = "local";
            CurrentController = "editions";
        }
    }
}
