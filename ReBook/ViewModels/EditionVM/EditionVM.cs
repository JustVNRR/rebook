namespace ReBook.ViewModels.EditionVM
{
    public class EditionVM
    {

        public string PageTitle { get; set; }
        public Edition Edition { get; set; }
        public string Error { get; set; }
        public string Repository { get; set; }
        public bool IsUserLookingFor { get; set; }
        public bool IsUserOwnerOf { get; set; }
        public string CurrentController { get; set; }

        public int AvailableCopies { get; set; }

        public EditionVM()
        {
            Edition = new Edition();
            PageTitle = "Unknown Title";
            Error = null;
            Repository = "local";
            IsUserLookingFor = false;
            IsUserOwnerOf = false;
            CurrentController = "editions";
            AvailableCopies = 0;
        }
    }
}
