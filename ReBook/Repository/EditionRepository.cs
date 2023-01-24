namespace ReBook.Repository
{
    public class EditionRepository : IEditionRepository
    {
        private readonly Cloudinary _cloudinary;
        private readonly ReBookDbContext _db;
        public EditionRepository(ReBookDbContext db, Cloudinary cloudinary)
        {
            _db = db;
            _cloudinary = cloudinary;
        }

        public List<EditionPartialVM> GetAll(QueryVM search)
        {
            return _db.Editions
                .OrderBy(e => e.Book.Title)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                .ToList();
        }

        public List<EditionPartialVM> GetAllWished(QueryVM search)
        {
            List<int> wished = _db.Wishes.Select(e => e.EditionId).Distinct().ToList();

            if (wished.Any())
            {
                return _db.Editions
                                .Where(e => wished.Contains(e.Id))
                                .OrderBy(e => e.Book.Title)
                                .Skip(search.CurrentPage * search.PageSize)
                                .Take(search.PageSize)
                                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                                .ToList();
            }
            return null;
        }

        public List<EditionPartialVM> GetAllAvailables(QueryVM search)
        {
            List<int> wished = _db.Copies.Where(c => c.Avalaible == true).Select(c => c.EditionId).Distinct().ToList();

            if (wished.Any())
            {
                return _db.Editions
                                .Where(e => wished.Contains(e.Id))
                                .OrderBy(e => e.Book.Title)
                                .Skip(search.CurrentPage * search.PageSize)
                                .Take(search.PageSize)
                                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                                .ToList();
            }
            return null;
        }
        public List<EditionPartialVM> Get(QueryVM search)
        {
            string query = search.QuerySearch;
            int skipRows = search.CurrentPage * search.PageSize;
            int pageSize = search.PageSize;

            return _db.Editions
                .Include(e => e.Book.Authors)
                .Where(e => (e.Book.Title.Contains(query) ||
                                e.Book.Authors.Any(a => a.Name.Contains(query) ||
                                e.Editor.Contains(query) ||
                                e.ISBN13.Contains(query) ||
                                e.Book.Tags.Any(a => a.Name.Contains(query)))))
                .OrderBy(e => e.Book.Title)
                .Skip(skipRows)
                .Take(pageSize)
                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                .ToList();
        }

        public List<EditionPartialVM> GetWished(QueryVM search)
        {
            List<int> wished = _db.Wishes.Select(e => e.EditionId).Distinct().ToList();
            if (wished.Any())
            {
                string query = search.QuerySearch;
                int skipRows = search.CurrentPage * search.PageSize;
                int pageSize = search.PageSize;

                return _db.Editions
                    .Include(e => e.Book.Authors)
                    .Where(e => (wished.Contains(e.Id) && (e.Book.Title.Contains(query) ||
                                    e.Book.Authors.Any(a => a.Name.Contains(query) ||
                                    e.Editor.Contains(query) ||
                                    e.ISBN13.Contains(query) ||
                                    e.Book.Tags.Any(a => a.Name.Contains(query))))))
                    .OrderBy(e => e.Book.Title)
                    .Skip(skipRows)
                    .Take(pageSize)
                    .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                    .ToList();
            }
            return null;
        }

        public List<EditionPartialVM> GetAvailables(QueryVM search)
        {
            List<int> wished = _db.Copies.Where(c => c.Avalaible == true).Select(e => e.EditionId).Distinct().ToList();
            if (wished.Any())
            {
                string query = search.QuerySearch;
                int skipRows = search.CurrentPage * search.PageSize;
                int pageSize = search.PageSize;

                return _db.Editions
                    .Include(e => e.Book.Authors)
                    .Where(e => (wished.Contains(e.Id) && (e.Book.Title.Contains(query) ||
                                    e.Book.Authors.Any(a => a.Name.Contains(query) ||
                                    e.Editor.Contains(query) ||
                                    e.ISBN13.Contains(query) ||
                                    e.Book.Tags.Any(a => a.Name.Contains(query))))))
                    .OrderBy(e => e.Book.Title)
                    .Skip(skipRows)
                    .Take(pageSize)
                    .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                    .ToList();
            }
            return null;
        }
        public List<EditionPartialVM> GetAdvanced(QueryVM search)
        {
            if (search.ISBN != null && search.ISBN.Length == 13)
            {
                return _db.Editions
                .Include(e => e.Book.Authors)
                .Where(e => (e.ISBN13.Equals(search.ISBN)))
                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                .ToList();
            }

            return _db.Editions
                            .Include(e => e.Book.Authors)
                            .Include(e => e.Book.Tags)
                            .Where(e => (
                                            (search.Lang == null || e.Language == search.Lang) &&
                                            (search.Title == null || e.Book.Title.Contains(search.Title)) &&
                                            (search.Editor == null || e.Editor.Contains(search.Editor)) &&
                                            (search.Author == null || e.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                            (search.Tag == null || e.Book.Tags.Any(t => t.Name.Contains(search.Tag)))))
                            .OrderBy(e => e.Book.Title)
                            .Skip(search.CurrentPage * search.PageSize)
                            .Take(search.PageSize)
                            .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                            .ToList();
        }

        public List<EditionPartialVM> GetAdvancedWished(QueryVM search)
        {
            List<int> wished = _db.Wishes.Select(e => e.EditionId).Distinct().ToList();
            if (wished.Any())
            {
                if (search.ISBN != null && search.ISBN.Length == 13)
                {
                    return _db.Editions
                    .Include(e => e.Book.Authors)
                    .Where(e => (wished.Contains(e.Id) && e.ISBN13.Equals(search.ISBN)))
                    .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                    .ToList();
                }

                return _db.Editions
                                .Include(e => e.Book.Authors)
                                .Include(e => e.Book.Tags)
                                .Where(e => (
                                                 wished.Contains(e.Id) &&
                                                (search.Lang == null || e.Language == search.Lang) &&
                                                (search.Title == null || e.Book.Title.Contains(search.Title)) &&
                                                (search.Editor == null || e.Editor.Contains(search.Editor)) &&
                                                (search.Author == null || e.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                                (search.Tag == null || e.Book.Tags.Any(t => t.Name.Contains(search.Tag)))))
                                .OrderBy(e => e.Book.Title)
                                .Skip(search.CurrentPage * search.PageSize)
                                .Take(search.PageSize)
                                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                                .ToList();
            }
            return null;
        }
        public List<EditionPartialVM> GetAdvancedAvailables(QueryVM search)
        {
            List<int> wished = _db.Copies.Where(c => c.Avalaible == true).Select(e => e.EditionId).Distinct().ToList();
            if (wished.Any())
            {
                if (search.ISBN != null && search.ISBN.Length == 13)
                {
                    return _db.Editions
                    .Include(e => e.Book.Authors)
                    .Where(e => (wished.Contains(e.Id) && e.ISBN13.Equals(search.ISBN)))
                    .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                    .ToList();
                }

                return _db.Editions
                                .Include(e => e.Book.Authors)
                                .Include(e => e.Book.Tags)
                                .Where(e => (
                                                 wished.Contains(e.Id) &&
                                                (search.Lang == null || e.Language == search.Lang) &&
                                                (search.Title == null || e.Book.Title.Contains(search.Title)) &&
                                                (search.Editor == null || e.Editor.Contains(search.Editor)) &&
                                                (search.Author == null || e.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                                (search.Tag == null || e.Book.Tags.Any(t => t.Name.Contains(search.Tag)))))
                                .OrderBy(e => e.Book.Title)
                                .Skip(search.CurrentPage * search.PageSize)
                                .Take(search.PageSize)
                                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                                .ToList();
            }
            return null;
        }
        public Edition GetByISBN(string ISBN)
        {
            return _db.Editions.Include(e => e.Book)
                                .Include(e => e.Book.Authors)
                                .Include(e => e.Book.Tags)
                                    .Where(e => (e.ISBN13.Equals(ISBN))).FirstOrDefault();
        }

        public Edition Insert(Edition edition)
        {
            string ISBN = edition.ISBN13;

            //L'édition existe-t-elle déjà en base?
            if (GetByISBN(ISBN) != null) return null; // L'édition est déjà dans la base...

            // On sauve les tags pour plus tard au cas ou il leur arriverait des misères...
            List<Tag> clonedTags = new List<Tag>(edition.Book.Tags);

            //L'édition n'existe pas en base, mais qu'en est t-il du livre?
            //On considère qu'il existe déjà dès lors que le titre et au moins un des auteurs correspondent
            // MAis maintant on sait qu'on peut overider une méthode EQUALS.... A FAIRE

            //A FAIRE : GERER LES EXECPTIONS... TRY CATCH TA MERE ELLE FAIT DES TRUCS pas NETS EN ENFER....
            var sameBooks = _db.Books.Include(b => b.Authors).Include(b => b.Tags)
                                    .Where(b => (b.Title.Equals(edition.Book.Title)
                                        && b.Authors.Any(a => a.Name.Equals(edition.Book.Authors[0].Name))));

            if (sameBooks.Any())
            {
                edition.Book = sameBooks.FirstOrDefault();
            }
            else
            {   //Le livre n'existe pas encore en base. Mais qu'en est-il des auteurs?
                // On skipe les homonymes... (mais idéalemement il faudrait rajouter le doublon avec un flag pour vérification humaine ultérieure.
                for (int i = 0; i < edition.Book.Authors.Count; i++)
                {
                    edition.Book.Authors[i] = _db.Authors.SingleOrDefault(author => author.Name == edition.Book.Authors[i].Name) ?? edition.Book.Authors[i];
                }
            }

            _db.Editions.Add(edition);
            _db.SaveChanges();

            //Et ces putains de tags?
            for (int i = 0; i < clonedTags.Count; i++)
            {
                if (edition.Book.Tags.Find(tag => tag.Name == clonedTags[i].Name) == null)
                {
                    if (_db.Tags.Find(clonedTags[i].Name) == null)
                    {
                        edition.Book.Tags.Add(clonedTags[i]);
                    }
                    else
                    {
                        _db.Tags.SingleOrDefault(t => t.Name == clonedTags[i].Name).Books.Add(edition.Book);
                    }
                }
            }
            _db.SaveChanges();
            return _db.Editions.Include(e => e.Book)
                                .Include(e => e.Book.Authors)
                                .Include(e => e.Book.Tags)
                                 .Where(o => o.ISBN13 == ISBN)
                                 .FirstOrDefault();
        }

        public Edition Update(Edition edition)
        {
            var existingEdition = _db.Editions.Include(e => e.Book)
                                            .Include(e => e.Book.Authors)
                                            .Include(e => e.Book.Tags)
                                             .Where(o => o.Id == edition.Id)
                                             .FirstOrDefault();

            existingEdition.Book.Title = edition.Book.Title;
            if (existingEdition != null)
            {
                _db.Entry(existingEdition).CurrentValues.SetValues(edition);

                existingEdition.Book.Tags.Clear();
                existingEdition.Book.Authors.Clear();

                _db.SaveChanges();

                foreach (var tag in edition.Book.Tags)
                {
                    var existingTag = _db.Tags.Find(tag.Name);

                    if (existingTag != null)
                    {
                        existingTag.Books.Add(existingEdition.Book);
                    }
                    else
                    {
                        existingEdition.Book.Tags.Add(tag);
                    }
                }

                foreach (var author in edition.Book.Authors)
                {
                    var existingAuthor = _db.Authors.Find(author.Id);

                    if (existingAuthor != null)
                    {
                        existingAuthor.Books.Add(existingEdition.Book);
                    }
                    else
                    {
                        existingEdition.Book.Authors.Add(author);
                    }
                }
                _db.SaveChanges();
            }

            return _db.Editions.Include(e => e.Book)
                                            .Include(e => e.Book.Authors)
                                            .Include(e => e.Book.Tags)
                                             .Where(e => e.Id == edition.Id)
                                             .FirstOrDefault();
        }

        public int DeleteById(int id)
        {
            //Faudra refaire un tour par ici quand on aura des exemplaires qui pointeront sur l'edition qu'on veut effacer
            var edition = _db.Editions.FirstOrDefault(e => e.Id == id);

            if (edition == null) return -1;

            DeleteCover(edition.Cover);

            _db.Editions.Remove(edition);
            return _db.SaveChanges();
        }

        public Wish AddWish(Wish wish)
        {
            _db.Wishes.Add(wish);
            _db.SaveChanges();
            return _db.Wishes.Where(w => w.UserId == wish.UserId && w.EditionId == wish.EditionId).FirstOrDefault();
        }

        // Pour l'autocompletion...
        public Edition GetById(int id)
        {
            return _db.Editions.Include(e => e.Book).Include(e => e.Book.Authors).Include(e => e.Book.Tags).Where(e => e.Id == id).FirstOrDefault();
        }
        public List<string> GetAuthorsList()
        {
            return _db.Authors.Select(a => a.Name).Where(s => s != null).Distinct().ToList();
        }
        public List<string> GetEditorsList()
        {
            return _db.Editions.Select(e => e.Editor).Where(s => s != null).Distinct().ToList();
        }

        public List<string> GetBooksList()
        {
            return _db.Books.Select(b => b.Title).Where(s => s != null).Distinct().ToList();
        }

        public List<string> GetTagsList()
        {
            return _db.Tags.Select(t => t.Name).Where(s => s != null).Distinct().ToList();
        }


        public bool IsUserLookingFor(int editionID, string userId)
        {
            if (_db.Wishes.Where(w => w.EditionId == editionID && w.UserId == userId).FirstOrDefault() == null)
            {
                return false;
            }

            return true;
        }

        public bool IsUserOwnerOf(int editionID, string userId)
        {
            if (_db.Copies.Where(c => c.EditionId == editionID && c.OwnerId == userId).FirstOrDefault() == null)
            {
                return false;
            }

            return true;
        }

        public int GetAvailableCopiesCount(int editionId)
        {
            return _db.Copies.Where(c => c.EditionId == editionId && c.Avalaible == true).Count();
        }

        public string SaveCover(string fileName, IFormFile file)
        {
            if (file == null) return null;

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, stream),
                    PublicId = fileName
                };

                var uploadResult = _cloudinary.Upload(uploadParams);
                return uploadResult.SecureUrl.AbsoluteUri;
            };
        }

        public string DeleteCover(string url)
        {
            if (url.IndexOf("cloudinary") != -1 && url.IndexOf("book-placeholder") == -1)
            {
                string publicId = "Editions/" + url.Split('/').Last().Split('.').First();

                return _cloudinary.Destroy(new DeletionParams(publicId)).ToString();
            }
            return "";
        }


    }
}
