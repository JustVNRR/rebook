namespace ReBook.ViewModels.AdministrationVM
{
    public class UserIndexVM
    {
        public List<UserPartialVM> Users { get; set; }
        public QueryUserVM Query { get; set; }

        public string Error { get; set; }

        public UserIndexVM()
        {
            Users = new List<UserPartialVM>();
            Query = new QueryUserVM();
            Error = null;
        }
    }
}
