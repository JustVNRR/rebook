namespace ReBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string Avatar { get; set; }
        public List<Wish> Wishes { get; set; }

        public float Rating { get; set; }
        public int NumberOfRatings { get; set; }

        public ApplicationUser()
        {
            Avatar = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624735508/Avatars/anonymous_placeholder_szrplp.jpg";
            Wishes = new List<Wish>();
        }
    }
}
