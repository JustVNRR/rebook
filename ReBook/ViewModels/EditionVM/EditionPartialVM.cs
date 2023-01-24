namespace ReBook.ViewModels.EditionVM
{
    public class EditionPartialVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public string ISBN { get; set; }

        public EditionPartialVM()
        {
            Id = 0;
            Cover = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
        }
    }
}
