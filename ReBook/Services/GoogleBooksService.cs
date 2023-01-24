using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;

namespace ReBook.Services
{
    public class GoogleBooksService : IEditionAPIService
    {
        private readonly BooksService _booksService;

        public GoogleBooksService(BooksService booksService)
        {
            _booksService = booksService;
        }

        private Edition ConvertVolumeInEdition(Volume volume)
        {
            var infos = volume.VolumeInfo;

            Edition edition = new Edition();

            var identifiers = infos.IndustryIdentifiers;
            if (identifiers == null) return null;

            edition.ISBN13 = null;

            for (int i = 0; i < identifiers.Count; i++)
            {
                if (identifiers[i].Type == "ISBN_13")
                {
                    edition.ISBN13 = Regex.Replace(identifiers[i].Identifier, "[^0-9]", "");
                    break;
                }
            }

            if (edition.ISBN13 == null || edition.ISBN13.Length != 13) return null;

            if (infos.IndustryIdentifiers == null) return null;

            edition.Book = new Book();
            edition.Book.Authors = new List<Author>();
            edition.Editor = infos.Publisher;

            edition.Book.Title = infos.Title;

            if (infos.Authors != null)
            {
                for (int a = 0; a < infos.Authors.Count; a++)
                {
                    edition.Book.Authors.Add(new Author());
                    edition.Book.Authors[a].Name = infos.Authors[a];
                }
            }

            if (infos.Description != null)
            {
                edition.Description = infos.Description;

            }
            if (infos.PublishedDate != null)
            {
                edition.publishedDate = infos.PublishedDate;
            }

            if (volume.VolumeInfo.PageCount != null)
            {
                edition.pageCount = (int)infos.PageCount; // faudra probablement changer le type en int? dans le modèle...
            }

            if (infos.ImageLinks != null && infos.ImageLinks.Thumbnail != null)
            {
                edition.Cover = infos.ImageLinks.Thumbnail;
            }

            if (infos.Language != null)
            {
                edition.Language = Languages.GetLanguageFromCode(infos.Language);
            }
            else
            {
                edition.Language = Language.Unknown;
            }

            return edition;
        }

        private EditionPartialVM ConvertVolumeInEditionPartial(Volume volume)
        {
            var infos = volume.VolumeInfo;

            EditionPartialVM edition = new EditionPartialVM();

            var identifiers = infos.IndustryIdentifiers;
            if (identifiers == null) return null;

            edition.ISBN = null;

            for (int i = 0; i < identifiers.Count; i++)
            {
                if (identifiers[i].Type == "ISBN_13")
                {
                    edition.ISBN = Regex.Replace(identifiers[i].Identifier, "[^0-9]", "");
                    break;
                }
            }

            if (edition.ISBN == null || edition.ISBN.Length != 13) return null;

            //edition.Authors = new List<Author>();
            edition.Title = infos.Title;

            /*if (volume.VolumeInfo.Authors != null)
            {
                for (int a = 0; a < infos.Authors.Count; a++)
                {
                    edition.Authors.Add(new Author());
                    edition.Authors[a].Name = volume.VolumeInfo.Authors[a];
                }
            }*/

            if (infos.ImageLinks != null && infos.ImageLinks.Thumbnail != null)
            {
                edition.Cover = infos.ImageLinks.Thumbnail;
            }

            return edition;
        }

        public IEnumerable<Edition> Get(string query, int offset, int count)
        {
            string formatedQuery = "q=" + query.Trim();
            var listquery = _booksService.Volumes.List(formatedQuery);
            listquery.Fields = "items(volumeInfo/title,volumeInfo/authors,volumeInfo/publisher,volumeInfo/publishedDate,volumeInfo/industryIdentifiers,volumeInfo/pageCount,volumeInfo/imageLinks,volumeInfo/language)";
            listquery.MaxResults = count;
            listquery.StartIndex = offset;
            var volumes = listquery.Execute();

            if (volumes.Items == null) return null;

            List<Volume> volumesList = volumes.Items.ToList();

            List<Edition> editions = new List<Edition>();

            for (int v = 0; v < volumesList.Count; v++)
            {
                editions.Add(ConvertVolumeInEdition(volumesList[v]));
            }

            return editions.Count() > 0 ? editions : null;
        }

        public List<EditionPartialVM> GetPartial(string query, int offset, int count)
        {
            string formatedQuery = "q=" + query.Trim();
            var listquery = _booksService.Volumes.List(formatedQuery);
            listquery.Fields = "items(volumeInfo/title,volumeInfo/industryIdentifiers,volumeInfo/imageLinks)";
            listquery.MaxResults = count;
            listquery.StartIndex = offset;
            var volumes = listquery.Execute();

            if (volumes.Items == null) return null;

            List<Volume> volumesList = volumes.Items.ToList();

            List<EditionPartialVM> editions = new List<EditionPartialVM>();

            for (int v = 0; v < volumesList.Count; v++)
            {
                editions.Add(ConvertVolumeInEditionPartial(volumesList[v]));
            }

            return editions.Count > 0 ? editions : null;
        }

        public async Task<EditionIndexVM> GetAdvanced(QueryVM search)
        {
            // Parce que Google Books API c'est de la GROSSSSSSSSSSSSSSSSSSE MERDE...

            if (!String.IsNullOrEmpty(search.QuerySearch))
            {
                search.QuerySearch = search.QuerySearch.Trim();

                var splitted = search.QuerySearch.Split('+');

                if (splitted.Length > 1)
                {
                    search.SearchContext = "advanced";
                    search.QuerySearch = null;

                    foreach (var item in splitted)
                    {
                        if (item.IndexOf("isbn:") != -1)
                        {
                            search.ISBN = item.Replace("isbn:", "");
                        }

                        if (item.IndexOf("intitle:") != -1)
                        {
                            search.Title = item.Replace("intitle:", "");
                        }

                        if (item.IndexOf("inauthor:") != -1)
                        {
                            search.Author = item.Replace("inauthor:", "");
                        }

                        if (item.IndexOf("inpublisher:") != -1)
                        {
                            search.Editor = item.Replace("inpublisher:", "");
                        }
                    }
                }
                else
                {
                    search.Title = search.QuerySearch.Trim().Replace("\"", "").Replace("intitle", "");
                    //search.Author = search.QuerySearch.Trim().Replace("\"", "");
                    search.SearchContext = "advanced";
                    search.QuerySearch = null;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(search.ISBN))
                {
                    search.ISBN = Regex.Replace(search.ISBN, "[^0-9]", "");

                    if (search.ISBN.Length == 10)
                    {
                        search.ISBN = ISBN10.Convert(search.ISBN);
                    }
                }

                /*if (!String.IsNullOrEmpty(search.Title)) search.Title = "\u0022" + search.Title.Trim().Replace("\"", "") + "\u0022";
                if (!String.IsNullOrEmpty(search.Author)) search.Author = "\u0022" + search.Author.Trim().Replace("\"", "") + "\u0022";
                if (!String.IsNullOrEmpty(search.Editor)) search.Editor = "\u0022" + search.Editor.Trim().Replace("\"", "") + "\u0022";
                if (!String.IsNullOrEmpty(search.QuerySearch)) search.QuerySearch = "\u0022" + search.QuerySearch.Trim().Replace("\"", "") + "\u0022";*/

                if (!String.IsNullOrEmpty(search.Title)) search.Title = search.Title.Trim().Replace("\"", "");
                if (!String.IsNullOrEmpty(search.Author)) search.Author = search.Author.Trim().Replace("\"", "");
                if (!String.IsNullOrEmpty(search.Editor)) search.Editor = search.Editor.Trim().Replace("\"", "");
                if (!String.IsNullOrEmpty(search.QuerySearch)) search.QuerySearch = search.QuerySearch.Trim().Replace("\"", "");

            }

            EditionIndexVM list = new EditionIndexVM();

            var query = "q=";

            if (!String.IsNullOrEmpty(search.ISBN))
            {
                var result = await GetByISBNPartial(search.ISBN);

                if (result != null)
                {
                    list.Editions.Add(result);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(search.QuerySearch))
                {
                    query += search.QuerySearch;
                }

                if (!String.IsNullOrEmpty(search.Title))
                {
                    query += "+intitle:" + search.Title;
                }

                if (!String.IsNullOrEmpty(search.Author))
                {
                    query += "+inauthor:" + search.Author;
                }

                if (!String.IsNullOrEmpty(search.Editor))
                {
                    query += "+inpublisher:" + search.Editor;
                }

                if (query.Length > 2)
                {
                    var listquery = _booksService.Volumes.List(query);

                    listquery.MaxResults = search.PageSize;
                    listquery.StartIndex = search.CurrentPage * search.PageSize;
                    listquery.Fields = "items(volumeInfo/title,volumeInfo/industryIdentifiers,volumeInfo/imageLinks)";
                    if (search.Lang != null)
                    {
                        var langCode = Languages.GetCodeFromName(search.Lang.ToString());

                        if (langCode != "un") listquery.LangRestrict = langCode;
                    }

                    var volumes = listquery.Execute();

                    if (volumes.Items != null)
                    {
                        List<Volume> volumesList = volumes.Items.ToList();

                        for (int v = 0; v < volumesList.Count; v++)
                        {
                            list.Editions.Add(ConvertVolumeInEditionPartial(volumesList[v]));
                        }
                    }
                }
            }

            search.CurrentPage++;
            search.Found = list.Editions.Count;
            list.Query = (QueryVM)search.Clone();

            return list;
        }

        public async Task<Edition> GetByISBN(string ISBN)
        {
            ISBN = Regex.Replace(ISBN, "[^0-9]", "");

            if (ISBN.Length == 10)
            {
                ISBN = ISBN10.Convert(ISBN);
            }

            if (ISBN.Length != 13) return null;

            string listquery = "isbn:" + ISBN;

            var volumes = await _booksService.Volumes.List(listquery).ExecuteAsync();

            if (volumes != null && volumes.Items != null)
            {
                foreach (var book in volumes.Items)
                {
                    var identifiers = book.VolumeInfo.IndustryIdentifiers;

                    if (identifiers == null) break;

                    for (int i = 0; i < identifiers.Count; i++)
                    {
                        if (identifiers[i].Type == "ISBN_13" && identifiers[i].Identifier == ISBN) return ConvertVolumeInEdition(book);
                    }
                }
                return null;
            }
            else { return null; }
        }

        public async Task<EditionPartialVM> GetByISBNPartial(string ISBN)
        {
            ISBN = Regex.Replace(ISBN, "[^0-9]", "");

            if (ISBN.Length == 10)
            {
                ISBN = ISBN10.Convert(ISBN);
            }

            if (ISBN.Length != 13) return null;

            string listquery = "isbn:" + ISBN;

            var volumes = await _booksService.Volumes.List(listquery).ExecuteAsync();

            if (volumes != null && volumes.Items != null)
            {
                foreach (var book in volumes.Items)
                {
                    var identifiers = book.VolumeInfo.IndustryIdentifiers;

                    if (identifiers == null) break;

                    for (int i = 0; i < identifiers.Count; i++)
                    {
                        if (identifiers[i].Type == "ISBN_13" && identifiers[i].Identifier == ISBN) return ConvertVolumeInEditionPartial(book);
                    }
                }
                return null;
            }
            else { return null; }
        }
    }
}
