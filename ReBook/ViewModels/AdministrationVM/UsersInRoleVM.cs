namespace ReBook.ViewModels.AdministrationVM
{
    public class UsersInRoleVM
    {
        public string PageTitle { get; set; }
        public string RoleName { get; set; }
        public string Error { get; set; }

        public List<UserRoleVM> Users { get; set; }

        public UsersInRoleVM()
        {
            Users = new List<UserRoleVM>();
            Error = null;
        }
    }
}
