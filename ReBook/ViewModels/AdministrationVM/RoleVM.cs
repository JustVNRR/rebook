namespace ReBook.ViewModels.AdministrationVM
{
    public class RoleVM
    {
        public string PageTitle { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }
        public RoleVM()
        {
            Error = null;
        }
    }
}
