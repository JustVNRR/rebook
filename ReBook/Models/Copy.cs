namespace ReBook.Models
{
    public enum Condition
    {
        NEW, FAIR, OLD
    }
    public class Copy
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("Edition")]
        public int EditionId { get; set; }
        public Edition Edition { get; set; }

        [ForeignKey("ApplicationUser")]
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public string Visuals { get; set; }

        [Display(Name = "I Want to trade this book")]
        public bool Avalaible { get; set; } = true;
        [Display(Name = "Condition :")]
        public Condition Condition { get; set; }

        [MaxLength(2000)]
        public string Comments { get; set; }

        public List<Pretender> Pretenders { get; set; }

        public List<Wish> Wishes { get; set; }
        public Copy()
        {
            Visuals = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
            Pretenders = new List<Pretender>();
            Wishes = new List<Wish>();
        }
    }
}
