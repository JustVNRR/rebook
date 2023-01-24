namespace ReBook.ViewModels.AdministrationVM
{
    public class QueryUserVM
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Found { get; set; }
        public string SearchContext { get; set; }
        public string Query { get; set; }
        public string Id { get; set; }
        public string Pseudo { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }

        public QueryUserVM()
        {
            SearchContext = "index";
            Query = null;
            Id = null;
            Pseudo = null;
            Login = null;
            Role = null;
            PageSize = 20;
            CurrentPage = 0;
            Found = 0;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
