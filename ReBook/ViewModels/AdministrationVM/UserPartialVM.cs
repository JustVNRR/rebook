namespace ReBook.ViewModels.AdministrationVM
{
    public class UserPartialVM
    {
        public string Id { get; set; }
        public string Pseudo { get; set; }
        public string Login { get; set; }
        public string Avatar { get; set; }

        public UserPartialVM()
        {
            Avatar = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624735508/Avatars/anonymous_placeholder_szrplp.jpg";
        }
    }
}
