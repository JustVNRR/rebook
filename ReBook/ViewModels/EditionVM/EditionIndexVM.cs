namespace ReBook.ViewModels.EditionVM
{
    public class EditionIndexVM
    {
        public List<EditionPartialVM> Editions { get; set; }
        public QueryVM Query { get; set; }

        public string CurrentController { get; set; }

        public EditionIndexVM()
        {
            Editions = new List<EditionPartialVM>();
            Query = new QueryVM();
            CurrentController = "editions";
        }
    }
}
