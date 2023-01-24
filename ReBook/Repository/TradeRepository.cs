namespace ReBook.Repository
{
    public class TradeRepository : ITradeRepository
    {
        private readonly ReBookDbContext _db;

        public TradeRepository(ReBookDbContext db)
        {
            _db = db;
        }

        public List<TradePartialVM> GetAllPartialByUserId(QueryTradeVM search)
        {
            string query = search.QuerySearch;
            string userId = search.UserId;

            if (search.StatusVM == TradeStatusVM.ALL)
            {
                return _db.Trades.Where(t => (t.FromUserId == userId || t.ToUserId == userId))
                .OrderByDescending(t => t.CreatedDate)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(t => new TradePartialVM()
                {
                    Id = t.Id,
                    FromUserID = t.FromUserId,
                    ToUserID = t.ToUserId,
                    FromUserName = t.FromUserName,
                    ToUserName = t.ToUserName,
                    FromBookTitle = t.FromCopyTitle,
                    ToBookTitle = t.ToCopyTitle,
                    FromCopyId = t.FromCopyId,
                    ToCopyId = t.ToCopyId,
                    FromCopyVisual = t.FromCopyVisual,
                    ToCopyVisual = t.ToCopyVisual,
                    Status = t.Status,
                    FromStatus = t.FromStatus,
                    ToStatus = t.ToStatus
                }).ToList();
            }
            else if (search.StatusVM == TradeStatusVM.WAITING)
            {
                return _db.Trades.Where(t => ((t.FromUserId == userId || t.ToUserId == userId) &&
                ((t.FromUserId == userId && t.ToStatus == TradeStatus.pending) || (t.ToUserId == userId && t.FromStatus == TradeStatus.pending))))
                .OrderByDescending(t => t.CreatedDate)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(t => new TradePartialVM()
                {
                    Id = t.Id,
                    FromUserID = t.FromUserId,
                    ToUserID = t.ToUserId,
                    FromUserName = t.FromUserName,
                    ToUserName = t.ToUserName,
                    FromBookTitle = t.FromCopyTitle,
                    ToBookTitle = t.ToCopyTitle,
                    FromCopyId = t.FromCopyId,
                    ToCopyId = t.ToCopyId,
                    FromCopyVisual = t.FromCopyVisual,
                    ToCopyVisual = t.ToCopyVisual,
                    Status = t.Status,
                    FromStatus = t.FromStatus,
                    ToStatus = t.ToStatus
                }).ToList();
            }
            else if (search.StatusVM == TradeStatusVM.TOSEND)
            {
                return _db.Trades.Where(t => ((t.FromUserId == userId || t.ToUserId == userId) &&
                ((t.FromUserId == userId && t.FromStatus == TradeStatus.pending) || (t.ToUserId == userId && t.ToStatus == TradeStatus.pending))))
                .OrderByDescending(t => t.CreatedDate)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(t => new TradePartialVM()
                {
                    Id = t.Id,
                    FromUserID = t.FromUserId,
                    ToUserID = t.ToUserId,
                    FromUserName = t.FromUserName,
                    ToUserName = t.ToUserName,
                    FromBookTitle = t.FromCopyTitle,
                    ToBookTitle = t.ToCopyTitle,
                    FromCopyId = t.FromCopyId,
                    ToCopyId = t.ToCopyId,
                    FromCopyVisual = t.FromCopyVisual,
                    ToCopyVisual = t.ToCopyVisual,
                    Status = t.Status,
                    FromStatus = t.FromStatus,
                    ToStatus = t.ToStatus
                }).ToList();
            }

            return _db.Trades.Where(t => ((t.FromUserId == userId || t.ToUserId == userId) && t.Status == TradeStatus.complete))
                .OrderByDescending(t => t.CreatedDate)
                .Skip(search.CurrentPage * search.PageSize)
                .Take(search.PageSize)
                .Select(t => new TradePartialVM()
                {
                    Id = t.Id,
                    FromUserID = t.FromUserId,
                    ToUserID = t.ToUserId,
                    FromUserName = t.FromUserName,
                    ToUserName = t.ToUserName,
                    FromBookTitle = t.FromCopyTitle,
                    ToBookTitle = t.ToCopyTitle,
                    FromCopyId = t.FromCopyId,
                    ToCopyId = t.ToCopyId,
                    FromCopyVisual = t.FromCopyVisual,
                    ToCopyVisual = t.ToCopyVisual,
                    Status = t.Status,
                    FromStatus = t.FromStatus,
                    ToStatus = t.ToStatus
                }).ToList();
        }

        public List<TradePartialVM> GetPartialByUserId(QueryTradeVM search)
        {
            string query = search.QuerySearch;
            string userId = search.UserId;
            List<int> copiesIds = _db.Copies
                .Where(c => ((c.Book.Title.Contains(query) ||
                                c.Book.Authors.Any(a => a.Name.Contains(query) ||
                                c.Edition.Editor.Contains(query) ||
                                c.Edition.ISBN13.Contains(query) ||
                                c.Book.Tags.Any(a => a.Name.Contains(query))))))
                .OrderBy(c => c.Book.Title)
                .Select(c => c.Id)
                .ToList();


            if (search.StatusVM == TradeStatusVM.ALL)
            {
                return _db.Trades
               .Where(t => ((t.FromUserId == userId || t.ToUserId == userId) &&
                   (copiesIds.Contains(t.FromCopyId) || copiesIds.Contains(t.ToCopyId))))
                   .OrderByDescending(t => t.CreatedDate)
                   .Skip(search.CurrentPage * search.PageSize)
                   .Take(search.PageSize)
                   .Select(t => new TradePartialVM()
                   {
                       Id = t.Id,
                       FromUserID = t.FromUserId,
                       ToUserID = t.ToUserId,
                       FromUserName = t.FromUserName,
                       ToUserName = t.ToUserName,
                       FromBookTitle = t.FromCopyTitle,
                       ToBookTitle = t.ToCopyTitle,
                       FromCopyId = t.FromCopyId,
                       ToCopyId = t.ToCopyId,
                       FromCopyVisual = t.FromCopyVisual,
                       ToCopyVisual = t.ToCopyVisual,
                       Status = t.Status,
                       FromStatus = t.FromStatus,
                       ToStatus = t.ToStatus
                   }).ToList();
            }
            else if (search.StatusVM == TradeStatusVM.WAITING)
            {
                return _db.Trades
               .Where(t => ((t.FromUserId == userId || t.ToUserId == userId) &&
                   (copiesIds.Contains(t.FromCopyId) || copiesIds.Contains(t.ToCopyId)) &&
                   ((t.FromUserId == userId && t.ToStatus == TradeStatus.pending) || (t.ToUserId == userId && t.FromStatus == TradeStatus.pending))))
                   .OrderByDescending(t => t.CreatedDate)
                   .Skip(search.CurrentPage * search.PageSize)
                   .Take(search.PageSize)
                   .Select(t => new TradePartialVM()
                   {
                       Id = t.Id,
                       FromUserID = t.FromUserId,
                       ToUserID = t.ToUserId,
                       FromUserName = t.FromUserName,
                       ToUserName = t.ToUserName,
                       FromBookTitle = t.FromCopyTitle,
                       ToBookTitle = t.ToCopyTitle,
                       FromCopyId = t.FromCopyId,
                       ToCopyId = t.ToCopyId,
                       FromCopyVisual = t.FromCopyVisual,
                       ToCopyVisual = t.ToCopyVisual,
                       Status = t.Status,
                       FromStatus = t.FromStatus,
                       ToStatus = t.ToStatus
                   }).ToList();
            }
            else if (search.StatusVM == TradeStatusVM.TOSEND)
            {
                return _db.Trades
               .Where(t => ((t.FromUserId == userId || t.ToUserId == userId) &&
                   (copiesIds.Contains(t.FromCopyId) || copiesIds.Contains(t.ToCopyId)) &&
                   ((t.FromUserId == userId && t.FromStatus == TradeStatus.pending) || (t.ToUserId == userId && t.ToStatus == TradeStatus.pending))))
                   .OrderByDescending(t => t.CreatedDate)
                   .Skip(search.CurrentPage * search.PageSize)
                   .Take(search.PageSize)
                   .Select(t => new TradePartialVM()
                   {
                       Id = t.Id,
                       FromUserID = t.FromUserId,
                       ToUserID = t.ToUserId,
                       FromUserName = t.FromUserName,
                       ToUserName = t.ToUserName,
                       FromBookTitle = t.FromCopyTitle,
                       ToBookTitle = t.ToCopyTitle,
                       FromCopyId = t.FromCopyId,
                       ToCopyId = t.ToCopyId,
                       FromCopyVisual = t.FromCopyVisual,
                       ToCopyVisual = t.ToCopyVisual,
                       Status = t.Status,
                       FromStatus = t.FromStatus,
                       ToStatus = t.ToStatus
                   }).ToList();
            }

            return _db.Trades.Where(t => ((t.FromUserId == userId || t.ToUserId == userId) &&
                                            (copiesIds.Contains(t.FromCopyId) || copiesIds.Contains(t.ToCopyId)) &&
                                            t.Status == search.Status &&
                                            ((t.FromStatus == search.FromStatus && t.FromUserId == userId) || ((t.ToStatus == search.ToStatus && t.ToUserId == userId)))))
                                            .OrderByDescending(t => t.CreatedDate)
                                            .Skip(search.CurrentPage * search.PageSize)
                                            .Take(search.PageSize)
                                            .Select(t => new TradePartialVM()
                                            {
                                                Id = t.Id,
                                                FromUserID = t.FromUserId,
                                                ToUserID = t.ToUserId,
                                                FromUserName = t.FromUserName,
                                                ToUserName = t.ToUserName,
                                                FromBookTitle = t.FromCopyTitle,
                                                ToBookTitle = t.ToCopyTitle,
                                                FromCopyId = t.FromCopyId,
                                                ToCopyId = t.ToCopyId,
                                                FromCopyVisual = t.FromCopyVisual,
                                                ToCopyVisual = t.ToCopyVisual,
                                                Status = t.Status,
                                                FromStatus = t.FromStatus,
                                                ToStatus = t.ToStatus
                                            }).ToList();
        }

        public List<TradePartialVM> GetPartialAdvancedByUserId(QueryTradeVM search)
        {
            return _db.Trades
                .Where(t => ((t.FromUserId == search.UserId || t.ToUserId == search.UserId) &&
                        ((t.ToCopyTitle.Contains(search.Title) || t.FromCopyTitle.Contains(search.Title))) &&
                            t.Status == search.Status &&
                            t.FromStatus == search.FromStatus &&
                            t.ToStatus == search.ToStatus))
                    .OrderByDescending(t => t.CreatedDate)
                    .Skip(search.CurrentPage * search.PageSize)
                    .Take(search.PageSize)
                    .Select(t => new TradePartialVM()
                    {
                        Id = t.Id,
                        FromUserID = t.FromUserId,
                        ToUserID = t.ToUserId,
                        FromUserName = t.FromUserName,
                        ToUserName = t.ToUserName,
                        FromBookTitle = t.FromCopyTitle,
                        ToBookTitle = t.ToCopyTitle,
                        FromCopyId = t.FromCopyId,
                        ToCopyId = t.ToCopyId,
                        FromCopyVisual = t.FromCopyVisual,
                        ToCopyVisual = t.ToCopyVisual,
                        Status = t.Status,
                        FromStatus = t.FromStatus,
                        ToStatus = t.ToStatus
                    })
                     .ToList();
        }

        public int Insert(Trade trade)
        {
            Copy fromCopy = _db.Copies.Where(c => c.Id == trade.FromCopyId).FirstOrDefault();
            if (fromCopy == null) return -10;
            if (fromCopy.Avalaible == false) return -11;

            Copy toCopy = _db.Copies.Where(c => c.Id == trade.ToCopyId).FirstOrDefault();
            if (toCopy == null) return -20;
            if (toCopy.Avalaible == false) return -21;

            fromCopy.Avalaible = false;
            toCopy.Avalaible = false;

            _db.Trades.Add(trade);
            return _db.SaveChanges();
        }

        public Trade GetById(int id)
        {
            return _db.Trades.Where(t => t.Id == id).FirstOrDefault();
        }

        public Trade Update(Trade trade)
        {
            var existingTrade = _db.Trades.Where(t => t.Id == trade.Id).FirstOrDefault();

            if (existingTrade != null)
            {
                UpdateUserRating(trade.ToUserId);
                UpdateUserRating(trade.FromUserId);

                _db.Entry(existingTrade).CurrentValues.SetValues(trade);

                if (trade.ToStatus == TradeStatus.complete)
                {
                    Copy toCopy = _db.Copies.Include(c => c.Pretenders).Where(c => c.Id == trade.ToCopyId).FirstOrDefault();

                    toCopy.OwnerId = trade.FromUserId;
                    toCopy.Owner = _db.Users.Where(u => u.Id == trade.FromUserId).FirstOrDefault();

                    Wish wishes = _db.Wishes.Include(w => w.ListCopies).Where(w => w.EditionId == toCopy.EditionId && w.UserId == trade.FromUserId).FirstOrDefault();

                    if (wishes != null)
                    {
                        foreach (var copy in wishes.ListCopies)
                        {
                            Pretender pretender = _db.Pretenders.Where(p => p.CopyId == copy.Id).FirstOrDefault();
                            if (pretender != null)
                            {
                                _db.Pretenders.Remove(pretender);
                            }
                        }

                        wishes.ListCopies.Clear();

                        _db.Wishes.Remove(wishes);
                    }
                }

                if (trade.FromStatus == TradeStatus.complete)
                {
                    Copy fromCopy = _db.Copies.Include(c => c.Pretenders).Where(c => c.Id == trade.FromCopyId).FirstOrDefault();

                    fromCopy.OwnerId = trade.ToUserId;
                    fromCopy.Owner = _db.Users.Where(u => u.Id == trade.ToUserId).FirstOrDefault();

                    Wish wishes = _db.Wishes.Include(w => w.ListCopies).Where(w => w.EditionId == fromCopy.EditionId && w.UserId == trade.ToUserId).FirstOrDefault();

                    if (wishes != null)
                    {
                        foreach (var copy in wishes.ListCopies)
                        {
                            Pretender pretender = _db.Pretenders.Where(p => p.CopyId == copy.Id).FirstOrDefault();
                            if (pretender != null)
                            {
                                _db.Pretenders.Remove(pretender);
                            }
                        }

                        wishes.ListCopies.Clear();

                        _db.Wishes.Remove(wishes);
                    }
                }

                _db.SaveChanges(); //Il faudrait faire un commit général... genre transaction bancaire... qui annule tout en cas de pb... Est-ce le cas ici?
                return _db.Trades.Where(t => t.Id == trade.Id).FirstOrDefault();
            }
            return null;
        }

        private void UpdateUserRating(string userId)
        {
            ApplicationUser user = _db.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                List<Trade> toUserTrades = _db.Trades.Where(t => t.ToUserId == user.Id).ToList();

                int nTrades = 0;
                float sum = 0f;
                foreach (var tr in toUserTrades)
                {
                    if (tr.ToUserRating > 0)
                    {
                        nTrades++;
                        sum += tr.ToUserRating;
                    }
                }

                List<Trade> fromUserTrades = _db.Trades.Where(t => t.FromUserId == user.Id).ToList();

                foreach (var tr in fromUserTrades)
                {
                    if (tr.FromUserRating > 0)
                    {
                        nTrades++;
                        sum += tr.ToUserRating;
                    }
                }

                if (nTrades > 0)
                {
                    user.Rating = sum / nTrades;
                    user.NumberOfRatings = nTrades;
                }
                _db.SaveChanges();
            }
        }
    }
}
