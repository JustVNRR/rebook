namespace ReBook.ViewModels.HomeVM
{
    public class IndexVM
    {
        public string PageTitle { get; set; }
        public int PageLegnth { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<Edition> Editions { get; set; }
    }
}
