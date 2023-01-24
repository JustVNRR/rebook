namespace ReBook.ViewModels.CopyVM
{
    public class DeleteCopyVM
    {
        public string PageTitle { get; set; }
        public string Error { get; set; }
        public string CurrentController { get; set; }
        public int CopyId { get; set; }

        public DeleteCopyVM()
        {
            PageTitle = "Remove book";
            Error = null;
            CurrentController = "copies";
        }
    }
}
