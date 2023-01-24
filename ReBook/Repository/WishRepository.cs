namespace ReBook.Repository
{
    public class WishRepository : IWishRepository
    {
        private readonly ReBookDbContext _db;
        public WishRepository(ReBookDbContext db)
        {
            _db = db;
        }

        public Wish Add(Wish wish)
        {
            _db.Wishes.Add(wish);
            _db.SaveChanges();
            return _db.Wishes.Where(w => w.UserId == wish.UserId && w.EditionId == wish.EditionId).FirstOrDefault();
        }

        public int Delete(Wish wish)
        {
            Wish result = _db.Wishes.Include(w => w.ListCopies).Where(w => w.EditionId == wish.EditionId && w.UserId == wish.UserId).FirstOrDefault();

            if (result == null) return -1;

            foreach (var copy in result.ListCopies)
            {
                Pretender pretender = _db.Pretenders.Where(p => p.CopyId == copy.Id).FirstOrDefault();
                if (pretender != null)
                {
                    _db.Pretenders.Remove(pretender);
                }
            }

            result.ListCopies.Clear();

            _db.Wishes.Remove(result);
            return _db.SaveChanges();
        }

        public List<EditionPartialVM> GetAllByUserId(string userId, int skipRows, int pageSize)
        {
            //https://entityframework.net/knowledge-base/3491721/linq-to-entities---where-in-clause-in-query

            var wishesUserList = _db.Wishes.Where(w => w.UserId == userId).Select(w => w.EditionId).ToList();

            return _db.Editions.Where(e => wishesUserList.Contains(e.Id))
                .OrderBy(e => e.Book.Title)
                .Skip(skipRows)
                .Take(pageSize)
                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                .ToList();
        }

        public List<EditionPartialVM> GetByUserId(string userId, string query, int skipRows, int pageSize)
        {
            var wishesUserList = _db.Wishes.Where(w => w.UserId == userId).Select(w => w.EditionId).ToList();

            return _db.Editions
                .Where(e => (wishesUserList.Contains(e.Id) && (e.Book.Title.Contains(query) ||
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

        public List<EditionPartialVM> GetAdvancedByUserId(QueryVM search)
        {
            var wishesUserList = _db.Wishes.Where(w => w.UserId == search.UserId).Select(w => w.EditionId).ToList();

            if (search.ISBN != null && search.ISBN.Length == 13)
            {
                return _db.Editions
                .Where(e => (wishesUserList.Contains(e.Id) && e.ISBN13.Equals(search.ISBN)))
                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                .ToList();
            }

            return _db.Editions
                .Where(e => (wishesUserList.Contains(e.Id) &&
                                ((search.Lang == null || e.Language == search.Lang) &&
                                (search.Title == null || e.Book.Title.Contains(search.Title)) &&
                                (search.Editor == null || e.Editor.Contains(search.Editor)) &&
                                (search.Author == null || e.Book.Authors.Any(a => a.Name.Contains(search.Author))) &&
                                (search.Tag == null || e.Book.Tags.Any(t => t.Name.Contains(search.Tag))))))
                .OrderBy(e => e.Book.Title)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(e => new EditionPartialVM() { ISBN = e.ISBN13, Id = e.Id, Cover = e.Cover, Title = e.Book.Title })
                .ToList();
        }
    }
}
