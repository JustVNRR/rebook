namespace ReBook.ViewModels.CopyVM
{
    public class CopyVM
    {
        public string PageTitle { get; set; }
        public Copy Copy { get; set; }
        public string Error { get; set; }
        public string CurrentController { get; set; }
        public bool IsLookingForEdition { get; set; }
        public bool IsOwnerOf { get; set; }
        public bool IsInTrade { get; set; }


        public CopyVM()
        {
            Copy = new Copy();
            PageTitle = "Book Details";
            Error = null;
            CurrentController = "copies";
            IsLookingForEdition = false;
            IsOwnerOf = false;
            IsInTrade = false;
        }
    }
}
