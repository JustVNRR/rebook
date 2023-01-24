namespace ReBook.Models
{
    public class Wish
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Edition")]
        public int EditionId { get; set; }
        public Edition Edition { get; set; }
        public List<Copy> ListCopies { get; set; }

        public Wish()
        {
            ListCopies = new List<Copy>();
        }
        public Wish(ApplicationUser user, Edition edition)
        {
            User = user;
            UserId = User.Id;
            Edition = edition;
            EditionId = Edition.Id;
            ListCopies = new List<Copy>();
        }
    }
}
