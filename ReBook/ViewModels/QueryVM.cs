namespace ReBook.ViewModels
{
    public class QueryVM
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 10;
        public int Found { get; set; }
        public string SearchContext { get; set; } = "index";
        public string QuerySearch { get; set; }

        public string CurrentController { get; set; }

        [BindProperty]
        public string Repo { get; set; }
        public string[] Repos { get; set; } = { "local", "google", "wishes", "copies" };

        public string UserId { get; set; }

        public string OwnerId { get; set; }

        [BindProperty]
        public string Available { get; set; } = "All";
        public string[] Availables { get; set; } = { "All", "Availables", "Privates" };

        [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$",
         ErrorMessage = "Please enter 13 or 10 digits")]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Editor { get; set; }
        public string Tag { get; set; }
        public Language? Lang { get; set; }

        public QueryVM()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
