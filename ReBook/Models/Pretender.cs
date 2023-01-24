namespace ReBook.Models
{
    public class Pretender
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        [ForeignKey("Copy")]
        public int CopyId { get; set; }
    }
}
