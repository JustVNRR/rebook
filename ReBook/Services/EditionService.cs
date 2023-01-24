namespace ReBook.Services
{
    public class EditionService : IEditionService
    {
        public IEditionRepository _dao;

        public EditionService(IEditionRepository dao)
        {
            _dao = dao;
        }

        public Edition GetByISBN(string ISBN)
        {
            ISBN = Regex.Replace(ISBN, "[^0-9]", "");

            if (ISBN.Length == 10)
            {
                ISBN = ISBN10.Convert(ISBN);
            }

            return _dao.GetByISBN(ISBN);
        }
        public Edition GetById(int id)
        {
            return _dao.GetById(id);
        }

        public EditionIndexVM Get(QueryVM search)
        {
            bool emptySearch = SanitizeQuery(search);

            EditionIndexVM list = new ();

            if (emptySearch) search.SearchContext = "index";

            switch (search.SearchContext)
            {
                case "index":
                    list.Editions = _dao.GetAll(search);
                    break;
                case "simple":
                    if (!String.IsNullOrEmpty(search.QuerySearch))
                        list.Editions = _dao.Get(search);
                    break;
                default:
                    list.Editions = _dao.GetAdvanced(search);
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

        private bool SanitizeQuery(QueryVM search)
        {
            bool emptySearch = true;
            //search.QuerySearch = "isbn:9782226034533+intitle:Sleeping beauties+inauthor:Stephen King+inpublisher:Albin Michel";

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
                        search.SearchContext = "advanced";
                        search.QuerySearch = null;
                    }

                    if (item.IndexOf("intitle:") != -1)
                    {
                        search.Title = item.Replace("intitle:", "").Replace("\"", "");
                        search.SearchContext = "advanced";
                        search.QuerySearch = null;
                    }

                    if (item.IndexOf("inauthor:") != -1)
                    {
                        search.Author = item.Replace("inauthor:", "").Replace("\"", "");
                        search.SearchContext = "advanced";
                        search.QuerySearch = null;
                    }

                    if (item.IndexOf("inpublisher:") != -1)
                    {
                        search.Editor = item.Replace("inpublisher:", "").Replace("\"", "");
                        search.SearchContext = "advanced";
                        search.QuerySearch = null;
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
            return emptySearch;
        }

        public int Delete(int id)
        {
            return _dao.DeleteById(id);
        }

        public EditionIndexVM GetWished(QueryVM search)
        {
            bool emptySearch = SanitizeQuery(search);

            EditionIndexVM list = new EditionIndexVM();

            if (emptySearch) search.SearchContext = "index";

            switch (search.SearchContext)
            {
                case "index":
                    list.Editions = _dao.GetAllWished(search);
                    break;
                case "simple":
                    if (!String.IsNullOrEmpty(search.QuerySearch))
                        list.Editions = _dao.GetWished(search);
                    break;
                default:
                    list.Editions = _dao.GetAdvancedWished(search);
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

        public EditionIndexVM GetAvailables(QueryVM search)
        {
            bool emptySearch = SanitizeQuery(search);

            EditionIndexVM list = new EditionIndexVM();

            if (emptySearch) search.SearchContext = "index";

            switch (search.SearchContext)
            {
                case "index":
                    list.Editions = _dao.GetAllAvailables(search);
                    break;
                case "simple":
                    if (!String.IsNullOrEmpty(search.QuerySearch))
                        list.Editions = _dao.GetAvailables(search);
                    break;
                default:
                    list.Editions = _dao.GetAdvancedAvailables(search);
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

        public Edition Create(Edition edition)
        {
            // Il faudrait faire un methode sanitize qui n'autorise que certains caractères, met en minuscule ou majuscule... et una autre méthode pour reformater au chargement...
            if (edition.Editor != null) edition.Editor = edition.Editor.Trim();
            edition.Book.Title = edition.Book.Title.Trim();

            for (int i = 0; i < edition.Book.Authors.Count; i++)
            {
                edition.Book.Authors[i].Name = edition.Book.Authors[i].Name.Trim();
            }

            for (int i = 0; i < edition.Book.Tags.Count; i++)
            {
                edition.Book.Tags[i].Name = edition.Book.Tags[i].Name.Trim();
            }

            if (edition.Description != null && edition.Description.Length > 2000)
            {
                edition.Description = edition.Description.Substring(0, 1999);
            }
            return _dao.Insert(edition);
        }

        public Edition Update(Edition edition)
        {
            if (edition.Editor != null) edition.Editor = edition.Editor.Trim();
            edition.Book.Title = edition.Book.Title.Trim();

            for (int i = 0; i < edition.Book.Authors.Count; i++)
            {
                edition.Book.Authors[i].Name = edition.Book.Authors[i].Name.Trim();
            }

            for (int i = 0; i < edition.Book.Tags.Count; i++)
            {
                edition.Book.Tags[i].Name = edition.Book.Tags[i].Name.Trim();
            }

            return _dao.Update(edition);
        }

        public bool IsUserLookingFor(int editionID, string userId)
        {
            return _dao.IsUserLookingFor(editionID, userId);
        }

        public bool IsUserOwnerOf(int editionID, string userId)
        {
            return _dao.IsUserOwnerOf(editionID, userId);
        }

        public int GetAvailableCopiesCount(int editionId)
        {
            return _dao.GetAvailableCopiesCount(editionId);
        }
        public List<string> GetAuthorsList()
        {
            return _dao.GetAuthorsList();
        }

        public List<string> GetEditorsList()
        {
            return _dao.GetEditorsList();
        }

        public List<string> GetBooksList()
        {
            return _dao.GetBooksList();
        }

        public List<string> GetTagsList()
        {
            return _dao.GetTagsList();
        }

        public Wish AddWish(Wish wish)
        {
            return _dao.AddWish(wish);
        }
        public string SaveCover(string fileName, IFormFile file)
        {
            return _dao.SaveCover(fileName, file);
        }
    }
}
