namespace ReBook.Services
{
    public class CopyService : ICopyService
    {
        public ICopyRepository _dao;

        public CopyService(ICopyRepository dao)
        {
            _dao = dao;
        }

        public Copy GetById(int id)
        {
            return _dao.GetById(id);
        }

        public Copy Add(Copy copy)
        {
            return _dao.Insert(copy);
        }

        public int Delete(int id)
        {
            return _dao.DeleteById(id);
        }

        public Edition GetEditionById(int editionId)
        {
            return _dao.GetEditionById(editionId);
        }

        public CopyIndexVM Get(QueryVM search)
        {
            bool emptySearch = SanitizeQuery(search);

            CopyIndexVM list = new CopyIndexVM();

            if (emptySearch) search.SearchContext = "index";

            switch (search.SearchContext)
            {
                case "index":
                    list.Copies = _dao.GetAllPartialByOwnerId(search);
                    break;
                case "simple":
                    if (!String.IsNullOrEmpty(search.QuerySearch))
                        list.Copies = _dao.GetPartialByOwnerId(search);
                    break;
                default:
                    list.Copies = _dao.GetPartialAdvancedByOwnerId(search);
                    break;
            }

            if (list.Copies != null)
            {
                search.CurrentPage++;
                search.Found = list.Copies.Count;
            }
            else
            {
                list.Copies = new List<CopyPartialVM>();
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

            return emptySearch;
        }

        public bool IsCurrentUserLookingForRelatetdEdition(string id, int editionId)
        {
            return _dao.IsCurrentUserLookingForRelatetdEdition(id, editionId);
        }

        public Copy Update(Copy copy)
        {
            return _dao.Update(copy);
        }

        public AvailableCopiesVM GetAvailableCopies(int editionId, string userId)
        {
            return _dao.GetAvailableCopies(editionId, userId);
        }

        public int WishCopieAdd(int copyId, string userId)
        {
            return _dao.WishCopieAdd(copyId, userId);
        }

        public int WishCopieRemove(int copyId, string userId)
        {
            return _dao.WishCopieRemove(copyId, userId);
        }

        public List<Copy> GetAllAvailablesByOwnerId(string userId, int skipRows, int pageSize)
        {
            return _dao.GetAllAvailablesByOwnerId(userId, skipRows, pageSize);
        }

        public int FindMatch(string ownerId, string currentUserId)
        {
            return _dao.FindMatch(ownerId, currentUserId);
        }

        public List<string> FindEditionPretenders(int editionId)
        {
            return _dao.FindEditionPretenders(editionId);
        }
    }
}

