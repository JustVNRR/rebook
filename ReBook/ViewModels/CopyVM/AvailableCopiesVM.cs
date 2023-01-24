namespace ReBook.ViewModels.CopyVM
{
    public class AvailableCopiesVM
    {
        /*public string PageTitle { get; set; }

        public string Error { get; set; }

        public string CurrentController { get; set; }*/

        public int EditionId { get; set; }

        public List<AvailableCopyPartial> Copies { get; set; }

        public AvailableCopiesVM()
        {
            /*PageTitle = "Available Copies";
            Error = null;
            CurrentController = "copies";*/
            Copies = new List<AvailableCopyPartial>();
        }
    }
}
