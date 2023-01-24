namespace ReBook.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Display(Name = "Author(s)")]
        public List<Author> Authors { get; set; }
        [Display(Name = "Tags")]
        public List<Tag> Tags { get; set; }
        public Book()
        {
            Authors = new List<Author>();
            Tags = new List<Tag>();
        }
        public Book(string title) : base()
        {
            Title = title;
        }
    }
}
