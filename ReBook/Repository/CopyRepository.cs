namespace ReBook.Repository
{
    public class CopyRepository : ICopyRepository
    {
        private readonly Cloudinary _cloudinary;
        private readonly ReBookDbContext _db;

        public CopyRepository(ReBookDbContext db, Cloudinary cloudinary)
        {
            _db = db;
            _cloudinary = cloudinary;
        }

        public int DeleteById(int id)
        {
            var copy = _db.Copies.FirstOrDefault(e => e.Id == id);

            if (copy == null) return -1;

            DeleteVisuals(copy.Visuals);

            _db.Copies.Remove(copy);
            return _db.SaveChanges();
        }

        public List<CopyPartialVM> GetAdvanced(QueryVM search)
        {
            return _db.Copies
                           .Include(c => c.Edition.ISBN13)
                           .Include(c => c.Edition.Language)
                           .Include(c => c.Edition.Editor)
                           .Include(c => c.Book.Title)
                           .Include(c => c.Book.Authors)
                           .Include(c => c.Book.Tags)
                           .Where(c => (
                                           (search.ISBN == null || c.Edition.ISBN13 == search.ISBN) &&
                                           (search.Lang == null || c.Edition.Language == search.Lang) &&
                                           (search.Title == null || c.Book.Title.Contains(search.Title)) &&
                                           (search.Editor == null || c.Edition.Editor.Contains(search.Editor)) &&
                                           (search.Author == null || c.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                           (search.Tag == null || c.Book.Tags.Any(t => t.Name.Contains(search.Tag)))))
                           .OrderBy(c => c.Owner.UserName)
                           .Skip(search.CurrentPage * search.PageSize)
                           .Take(search.PageSize)
                           .Select(c => new CopyPartialVM()
                           {
                               Id = c.Id,
                               OwerID = c.OwnerId,
                               OwerPseudo = c.Owner.UserName,
                               BookTitle = c.Book.Title,
                               Visuals = c.Visuals,
                               EditionCover = c.Edition.Cover,
                               EditionId = c.EditionId
                           })
                           .ToList();
        }

        public List<CopyPartialVM> GetAll(int skipRows, int pageSize)
        {
            return _db.Copies
                .Include(c => c.Book.Title)
                .OrderBy(c => c.Owner.UserName)
                .Skip(skipRows)
                .Take(pageSize)
                .Select(c => new CopyPartialVM()
                {
                    Id = c.Id,
                    OwerID = c.OwnerId,
                    OwerPseudo = c.Owner.UserName,
                    BookTitle = c.Book.Title,
                    Visuals = c.Visuals,
                    EditionCover = c.Edition.Cover,
                    EditionId = c.EditionId
                })
                .ToList();
        }

        public Copy GetById(int id)
        {
            return _db.Copies.Include(c => c.Edition)
                                .Include(c => c.Book)
                                .Include(c => c.Book.Authors)
                                .Include(c => c.Owner)
                                .Include(c => c.Pretenders)
                .Where(c => c.Id == id).FirstOrDefault();
        }

        public Copy Insert(Copy copy)
        {
            _db.Copies.Add(copy);
            _db.SaveChanges();
            return _db.Copies.Include(c => c.Book)
                                 .Include(c => c.Edition)
                                  .Where(c => c.Id == copy.Id)
                                  .FirstOrDefault();
        }

        public Copy Update(Copy copy)
        {
            var existingCopy = _db.Copies.Where(c => c.Id == copy.Id).FirstOrDefault();

            if (existingCopy != null)
            {
                _db.Entry(existingCopy).CurrentValues.SetValues(copy);
                _db.SaveChanges();
                return _db.Copies.Include(e => e.Book)
                                    .Include(c => c.Edition)
                                        .Where(c => c.Id == copy.Id)
                                        .FirstOrDefault();
            }
            return null;
        }

        public string SaveVisuals(string fileName, IFormFile file)
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

        public string DeleteVisuals(string url)
        {
            if (url.IndexOf("cloudinary") != -1 && url.IndexOf("book-placeholder") == -1)
            {
                string publicId = "Copies/" + url.Split('/').Last().Split('.').First();

                return _cloudinary.Destroy(new DeletionParams(publicId)).ToString();
            }
            return "";
        }

        public Edition GetEditionById(int editionId)
        {
            return _db.Editions.Include(e => e.Book)
                                .Include(e => e.Book.Authors)
                                .Include(e => e.Book.Tags)
                                    .Where(e => (e.Id == editionId)).FirstOrDefault();
        }

        public List<CopyPartialVM> GetAllPartialByOwnerId(QueryVM search)
        {
            if (search.Available == "All")
            {
                return _db.Copies.Where(c => c.OwnerId == search.OwnerId)
                .OrderBy(c => c.Book.Title)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(c => new CopyPartialVM()
                {
                    Id = c.Id,
                    EditionCover = c.Edition.Cover,
                    Visuals = c.Visuals,
                    BookTitle = c.Book.Title,
                    OwerID = c.OwnerId,
                    OwerPseudo = c.Owner.UserName,
                    EditionId = c.EditionId
                })
                .ToList();
            }
            else
            {
                bool available = (search.Available == "Privates") ? false : true;

                return _db.Copies.Where(c => c.OwnerId == search.OwnerId && c.Avalaible == available)
                .OrderBy(c => c.Book.Title)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(c => new CopyPartialVM()
                {
                    Id = c.Id,
                    EditionCover = c.Edition.Cover,
                    Visuals = c.Visuals,
                    BookTitle = c.Book.Title,
                    OwerID = c.OwnerId,
                    OwerPseudo = c.Owner.UserName,
                    EditionId = c.EditionId
                })
                .ToList();
            }
        }

        public List<Copy> GetAllByOwnerId(string ownerId, int skipRows, int pageSize) //Pour le moment on tient pas compte de la pagination
        {

            return _db.Copies.Include(c => c.Edition)
                               .Include(c => c.Book)
                               .Include(c => c.Book.Authors)
                               .Include(c => c.Owner)
                               .Include(c => c.Pretenders)
                                .Where(c => c.OwnerId == ownerId).ToList();
        }

        public List<Copy> GetAllAvailablesByOwnerId(string ownerId, int skipRows, int pageSize) //Pour le moment on tient pas compte de la pagination
        {
            return _db.Copies.Include(c => c.Edition)
                               .Include(c => c.Book)
                               .Include(c => c.Book.Authors)
                               .Include(c => c.Owner)
                               .Include(c => c.Pretenders)
                                .Where(c => c.OwnerId == ownerId && c.Avalaible == true).ToList();
        }

        public List<CopyPartialVM> GetPartialByOwnerId(QueryVM search)
        {
            string query = search.QuerySearch;

            if (search.Available == "All")
            {
                return _db.Copies
                    .Where(c => (c.OwnerId == search.OwnerId && (c.Book.Title.Contains(query) ||
                                    c.Book.Authors.Any(a => a.Name.Contains(query) ||
                                    c.Edition.Editor.Contains(query) ||
                                    c.Edition.ISBN13.Contains(query) ||
                                    c.Book.Tags.Any(a => a.Name.Contains(query))))))
                    .OrderBy(c => c.Book.Title)
                    .Skip(search.CurrentPage * search.PageSize)
                    .Take(search.PageSize)
                    .Select(c => new CopyPartialVM()
                    {
                        Id = c.Id,
                        EditionCover = c.Edition.Cover,
                        Visuals = c.Visuals,
                        BookTitle = c.Book.Title,
                        OwerID = c.OwnerId,
                        OwerPseudo = c.Owner.UserName,
                        EditionId = c.EditionId
                    })
                    .ToList();
            }
            else
            {
                bool available = (search.Available == "Privates") ? false : true;

                return _db.Copies
                    .Where(c => (c.OwnerId == search.OwnerId && c.Avalaible == available && (c.Book.Title.Contains(query) ||
                                    c.Book.Authors.Any(a => a.Name.Contains(query) ||
                                    c.Edition.Editor.Contains(query) ||
                                    c.Edition.ISBN13.Contains(query) ||
                                    c.Book.Tags.Any(a => a.Name.Contains(query))))))
                    .OrderBy(c => c.Book.Title)
                    .Skip(search.CurrentPage * search.PageSize)
                    .Take(search.PageSize)
                    .Select(c => new CopyPartialVM()
                    {
                        Id = c.Id,
                        EditionCover = c.Edition.Cover,
                        Visuals = c.Visuals,
                        BookTitle = c.Book.Title,
                        OwerID = c.OwnerId,
                        OwerPseudo = c.Owner.UserName,
                        EditionId = c.EditionId
                    })
                    .ToList();
            }
        }

        public List<CopyPartialVM> GetPartialAdvancedByOwnerId(QueryVM search)
        {
            string query = search.QuerySearch;

            if (search.Available == "All")
            {
                if (search.ISBN != null && search.ISBN.Length == 13)
                {
                    return _db.Copies
                    .Where(c => (c.OwnerId == search.OwnerId && c.Edition.ISBN13.Equals(search.ISBN)))
                    .Select(c => new CopyPartialVM()
                    {
                        Id = c.Id,
                        EditionCover = c.Edition.Cover,
                        Visuals = c.Visuals,
                        BookTitle = c.Book.Title,
                        OwerID = c.OwnerId,
                        OwerPseudo = c.Owner.UserName,
                        EditionId = c.EditionId
                    })
                    .ToList();
                }

                return _db.Copies
                    .Where(c => (c.OwnerId == search.OwnerId &&
                                    ((search.Lang == null || c.Edition.Language == search.Lang) &&
                                    (search.Title == null || c.Book.Title.Contains(search.Title)) &&
                                    (search.Editor == null || c.Edition.Editor.Contains(search.Editor)) &&
                                    (search.Author == null || c.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                    (search.Tag == null || c.Book.Tags.Any(t => t.Name.Contains(search.Tag))))))
                    .OrderBy(e => e.Book.Title)
                    .Skip(search.CurrentPage * search.PageSize)
                    .Take(search.PageSize)
                        .Select(c => new CopyPartialVM()
                        {
                            Id = c.Id,
                            EditionCover = c.Edition.Cover,
                            Visuals = c.Visuals,
                            BookTitle = c.Book.Title,
                            OwerID = c.OwnerId,
                            OwerPseudo = c.Owner.UserName,
                            EditionId = c.EditionId
                        })
                    .ToList();
            }
            else
            {
                bool available = (search.Available == "Privates") ? false : true;

                if (search.ISBN != null && search.ISBN.Length == 13)
                {
                    return _db.Copies
                    .Where(c => (c.OwnerId == search.OwnerId && c.Avalaible == available && c.Edition.ISBN13.Equals(search.ISBN)))
                    .Select(c => new CopyPartialVM()
                    {
                        Id = c.Id,
                        EditionCover = c.Edition.Cover,
                        Visuals = c.Visuals,
                        BookTitle = c.Book.Title,
                        OwerID = c.OwnerId,
                        OwerPseudo = c.Owner.UserName,
                        EditionId = c.EditionId
                    })
                    .ToList();
                }

                return _db.Copies
                    .Where(c => (c.OwnerId == search.OwnerId && c.Avalaible == available &&
                                    ((search.Lang == null || c.Edition.Language == search.Lang) &&
                                    (search.Title == null || c.Book.Title.Contains(search.Title)) &&
                                    (search.Editor == null || c.Edition.Editor.Contains(search.Editor)) &&
                                    (search.Author == null || c.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                    (search.Tag == null || c.Book.Tags.Any(t => t.Name.Contains(search.Tag))))))
                    .OrderBy(e => e.Book.Title)
                    .Skip(search.CurrentPage * search.PageSize)
                    .Take(search.PageSize)
                        .Select(c => new CopyPartialVM()
                        {
                            Id = c.Id,
                            EditionCover = c.Edition.Cover,
                            Visuals = c.Visuals,
                            BookTitle = c.Book.Title,
                            OwerID = c.OwnerId,
                            OwerPseudo = c.Owner.UserName,
                            EditionId = c.EditionId
                        })
                    .ToList();
            }
        }

        public bool IsCurrentUserLookingForRelatetdEdition(string userId, int editionId)
        {
            return (_db.Wishes.Where(w => w.EditionId == editionId && w.UserId == userId).FirstOrDefault() != null);
        }

        public AvailableCopiesVM GetAvailableCopies(int editionId, string userId)
        {
            AvailableCopiesVM availables = new AvailableCopiesVM();

            availables.Copies = _db.Copies.Where(c => c.EditionId == editionId && c.Avalaible == true)
                .Select(c => new AvailableCopyPartial()
                {
                    CopyId = c.Id,
                    OwnerId = c.OwnerId,
                    Owner = c.Owner,
                    Visuals = c.Visuals,
                    Comments = c.Comments,
                    Condition = c.Condition
                }).ToList();

            if (availables.Copies.Any() && userId != null)
            {
                Wish wish = _db.Wishes.Include(w => w.ListCopies).Where(w => w.UserId == userId && w.EditionId == editionId).FirstOrDefault();

                if (wish != null)
                {
                    List<Copy> copies = wish.ListCopies.ToList();

                    if (copies.Any())
                    {
                        for (int i = 0; i < availables.Copies.Count; i++)
                        {
                            availables.Copies[i].isUserPretender = (copies.Where(c => c.Id == availables.Copies[i].CopyId).FirstOrDefault() != null);
                        }
                    }
                }
            }

            return availables;
        }

        public int WishCopieAdd(int copyId, string userId)
        {
            Copy copy = _db.Copies.Where(c => c.Id == copyId).FirstOrDefault();

            if (copy == null)
            {
                return -1;
            }

            ApplicationUser user = _db.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return -1;
            }

            var usersList = copy.Pretenders;
            usersList.Add(new Pretender { UserId = user.Id, UserName = user.UserName });

            Wish wish = _db.Wishes.Include(w => w.ListCopies).Where(w => w.UserId == userId && w.EditionId == copy.EditionId).FirstOrDefault();

            if (wish == null)
            {
                wish = new Wish { UserId = userId, User = user, Edition = copy.Edition, EditionId = copy.EditionId };
                wish.ListCopies.Add(copy);
                _db.Wishes.Add(wish);
            }
            else
            {
                Copy existingCopy = wish.ListCopies.Where(c => c.Id == copy.Id).FirstOrDefault();

                if (existingCopy == null)
                {
                    wish.ListCopies.Add(copy);
                }
            }

            return _db.SaveChanges();
        }

        public int WishCopieRemove(int copyId, string userId)
        {
            Copy copy = _db.Copies.Include(c => c.Pretenders).Where(c => c.Id == copyId).FirstOrDefault();

            if (copy == null)
            {
                return -1;
            }

            ApplicationUser user = _db.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return -1;
            }

            Pretender pretender = copy.Pretenders.Where(p => p.UserId == userId).FirstOrDefault();

            if (pretender != null)
            {
                _db.Pretenders.Remove(pretender);
            }

            Wish wish = _db.Wishes.Include(w => w.ListCopies).Where(w => w.UserId == userId && w.EditionId == copy.EditionId).FirstOrDefault();

            if (wish != null)
            {
                Copy wishCopy = wish.ListCopies.Where(c => c.Id == copyId).FirstOrDefault();

                if (wishCopy != null)
                {
                    wish.ListCopies.Remove(wishCopy);
                }
            }

            return _db.SaveChanges();
        }

        public int FindMatch(string ownerId, string currentUserId)
        {
            //Est ce que Owner s'est positionné sur au moins un bouquin de User? Et si oui le/les quel?

            List<Copy> copies = _db.Copies.Include(c => c.Pretenders).Where(c => c.OwnerId == currentUserId && c.Avalaible == true).ToList();

            foreach (var copy in copies)
            {
                if (copy.Pretenders.Any())
                {
                    foreach (var pretender in copy.Pretenders)
                    {
                        if (pretender.UserId == ownerId)
                        {
                            return copy.Id;
                        }
                    }
                }
            }
            return -1;
        }

        public List<string> FindEditionPretenders(int editionId)
        {
            var result = _db.Wishes.Where(w => w.EditionId == editionId).Select(w => w.UserId).ToList();
            return result;
        }
    }
}
