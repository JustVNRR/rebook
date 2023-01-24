namespace ReBook.ViewModels.CopyVM
{
    public class CopyIndexVM
    {
        public List<CopyPartialVM> Copies { get; set; }
        public QueryVM Query { get; set; }
        public string CurrentController { get; set; }

        public CopyIndexVM()
        {
            Copies = new List<CopyPartialVM>();
            Query = new QueryVM();
            CurrentController = "copies";
        }
    }
}
