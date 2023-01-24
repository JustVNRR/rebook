namespace ReBook.Services
{
    public class WishService :IWishService
    {
        public IWishRepository _dao;

        public WishService(IWishRepository dao)
        {
            _dao = dao;
        }

        public Wish Add(Wish wish)
        {
            return _dao.Add(wish);
        }
        public int Delete(Wish wish)
        {
            return _dao.Delete(wish);
        }

        public EditionIndexVM Get(QueryVM search)
        {
            bool emptySearch = true;

            if (!String.IsNullOrEmpty(search.QuerySearch))
            {
                emptySearch = false;
                search.QuerySearch = search.QuerySearch.Trim();

                var splitted = search.QuerySearch.Split('+');

                if (splitted.Length > 1)
                {
                    search.SearchContext = "advanced";
                    search.QuerySearch = null;
                }

                foreach (var item in splitted)
                {
                    if (item.IndexOf("isbn:") != -1)
                    {
                        search.ISBN = item.Replace("isbn:", "").Replace("\"", "");
                    }

                    if (item.IndexOf("intitle:") != -1)
                    {
                        search.Title = item.Replace("intitle:", "").Replace("\"", "");
                    }

                    if (item.IndexOf("inauthor:") != -1)
                    {
                        search.Author = item.Replace("inauthor:", "").Replace("\"", "");
                    }

                    if (item.IndexOf("inpublisher:") != -1)
                    {
                        search.Editor = item.Replace("inpublisher:", "").Replace("\"", "");
                    }
                }
            }

            if (!String.IsNullOrEmpty(search.ISBN))
            {
                emptySearch = false;
                search.ISBN = Regex.Replace(search.ISBN, "[^0-9]", "");

                if (search.ISBN.Length == 10)
                {
                    search.ISBN = ISBN10.Convert(search.ISBN);
                }
            }

            if (!String.IsNullOrEmpty(search.Title))
            {
                emptySearch = false;
                search.Title = search.Title.Trim();
            }
            if (!String.IsNullOrEmpty(search.Author))
            {
                emptySearch = false;
                search.Author = search.Author.Trim();
            }
            if (!String.IsNullOrEmpty(search.Editor))
            {
                emptySearch = false;
                search.Editor = search.Editor.Trim();
            }

            EditionIndexVM list = new EditionIndexVM();

            if (emptySearch == true) search.SearchContext = "index";

            switch (search.SearchContext)
            {
                case "index":
                    list.Editions = _dao.GetAllByUserId(search.UserId, search.CurrentPage * search.PageSize, search.PageSize);
                    break;
                case "simple":
                    if (!String.IsNullOrEmpty(search.QuerySearch))
                        list.Editions = _dao.GetByUserId(search.UserId, search.QuerySearch, search.CurrentPage * search.PageSize, search.PageSize);
                    break;
                default:
                    list.Editions = _dao.GetAdvancedByUserId(search);
                    break;
            }

            if (list.Editions != null)
            {
                search.CurrentPage++;
                search.Found = list.Editions.Count;
            }
            else
            {
                list.Editions = new List<EditionPartialVM>();
                search.CurrentPage = 0;
                search.Found = 0;
            }

            list.Query = (QueryVM)search.Clone();
            return list;
        }
    }
}
