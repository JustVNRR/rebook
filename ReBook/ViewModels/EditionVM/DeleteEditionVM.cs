namespace ReBook.ViewModels.EditionVM
{
    public class DeleteEditionVM
    {
        public string PageTitle { get; set; }
        public int Id { get; set; }
        public string Editor { get; set; }
        public string Book { get; set; }
        public string Cover { get; set; }
        public string ISBN { get; set; }

        public string Error { get; set; }
    }
}
