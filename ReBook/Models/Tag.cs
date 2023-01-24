namespace ReBook.Models
{
    public class Tag
    {
        [Key]
        [Display(Name = "Tag")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public Tag()
        {
            Books = new List<Book>();
        }
    }
}
