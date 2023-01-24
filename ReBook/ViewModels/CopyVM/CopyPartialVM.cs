namespace ReBook.ViewModels.CopyVM
{
    public class CopyPartialVM
    {
        public int Id { get; set; }
        public string OwerID { get; set; }
        public string OwerPseudo { get; set; }
        public string BookTitle { get; set; }
        public string EditionCover { get; set; }
        public string Visuals { get; set; }
        public int EditionId { get; set; }

        public CopyPartialVM()
        {
            EditionCover = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
            Visuals = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
        }
    }
}
