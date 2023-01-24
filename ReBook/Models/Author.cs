namespace ReBook.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [Display(Name = "Author")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }

        public Author(string name) : base()
        {
            Name = name;
        }

        public Author(string name, List<Book> books) : base()
        {
            Name = name;
            Books = books;
        }
    }
}
