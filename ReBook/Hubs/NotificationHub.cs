namespace ReBook.Hubs
{
    public class NotificationHub : Hub
    {
        public INotificationRepository _notifs;
        //public ReBookDbContext _db { get; }

        private readonly UserManager<ApplicationUser> userManager;

        public ICopyRepository _copies;
        public ITradeRepository _trades;

        public NotificationHub(UserManager<ApplicationUser> userManager,
            /*ReBookDbContext db,*/
            INotificationRepository dao,
            ICopyRepository copies,
            ITradeRepository trades)
        {
            this.userManager = userManager;
            _notifs = dao;
            //_db = db;
            _trades = trades;
            _copies = copies;
        }

        public async Task SendNotification(string toUserId, string fromUserId, int fromUserCopyId, int toUserCopyId, string currentUserId, NotiType type)
        {
            ApplicationUser fromUser = await userManager.FindByIdAsync(fromUserId);
            if (fromUser == null)
            {
                return;
            }
            ApplicationUser toUser = await userManager.FindByIdAsync(toUserId);
            if (toUser == null)
            {
                return;
            }

            Copy fromCopy = _copies.GetById(fromUserCopyId);
            Copy toCopy = _copies.GetById(toUserCopyId);

            Notification notification = new Notification
            {
                Type = type,
                FromUserId = fromUserId,
                FromUserName = fromUser.UserName,
                FromUserLogin = fromUser.Email,
                FromUserAddress = fromUser.Address,
                ToUserId = toUserId,
                ToUserName = toUser.UserName,
                ToUserLogin = toUser.Email,
                ToUserAddress = toUser.Address,
                ToCopyId = toUserCopyId,
                ToCopyTitle = toCopy.Book.Title,
            };

            if (type != NotiType.wish)
            {
                notification.FromCopyId = fromUserCopyId;
                notification.FromCopyTitle = fromCopy.Book.Title;
            }

            if (currentUserId == fromUserId)
            {
                notification.FromStatus = NotiStatus.accepted;
            }
            else if (currentUserId == toUserId)
            {
                notification.ToStatus = NotiStatus.accepted;
            }

            if (fromCopy != null)
            {
                if (fromCopy.Visuals.IndexOf("placeholder") == -1)
                {
                    notification.FromCopyVisual = fromCopy.Visuals;
                }
                else
                {
                    notification.FromCopyVisual = fromCopy.Edition.Cover;
                }
            }

            if (toCopy.Visuals.IndexOf("placeholder") == -1)
            {
                notification.ToCopyVisual = toCopy.Visuals;
            }
            else
            {
                notification.ToCopyVisual = toCopy.Edition.Cover;
            }

            if (_notifs.Create(notification) == 1)
            {
                int toCount = _notifs.GetUnreadUserNotificationsCount(toUserId);
                await Clients.User(toUserId).SendAsync("ReceiveNotification", toCount);

                int fromCount = _notifs.GetUnreadUserNotificationsCount(fromUserId);
                await Clients.User(fromUserId).SendAsync("ReceiveNotification", fromCount);
            }
        }

        public async Task SendUpdateTradeNotification(int tradeId, string currentUserId)
        {
            Trade trade = _trades.GetById(tradeId);

            if (trade == null)
            {
                return;
            }

            Notification notification = new Notification
            {
                Type = NotiType.updateTrade,
                FromUserId = trade.FromUserId,
                FromUserName = trade.FromUserName,
                FromUserLogin = trade.FromUserLogin,
                FromCopyId = trade.FromCopyId,
                FromCopyTitle = trade.FromCopyTitle,
                ToUserId = trade.ToUserId,
                ToUserName = trade.ToUserName,
                ToUserLogin = trade.ToUserLogin,
                ToCopyId = trade.ToCopyId,
                ToCopyTitle = trade.ToCopyTitle,
            };

            if (currentUserId == trade.FromUserId)
            {
                notification.FromStatus = NotiStatus.accepted;

                if (notification.Type == NotiType.updateTrade)
                {
                    notification.UserComment = trade.ToUserComment;
                    notification.UserRatings = trade.ToUserRating;
                    if (trade.ToStatus == TradeStatus.complete) notification.UserReceived = true;
                }
            }
            else if (currentUserId == trade.ToUserId)
            {
                notification.ToStatus = NotiStatus.accepted;

                if (notification.Type == NotiType.updateTrade)
                {
                    notification.UserComment = trade.FromUserComment;
                    notification.UserRatings = trade.FromUserRating;
                    if (trade.FromStatus == TradeStatus.complete) notification.UserReceived = true;
                }
            }

            if (_notifs.Create(notification) == 1)
            {
                int toCount = _notifs.GetUnreadUserNotificationsCount(trade.ToUserId);
                await Clients.User(trade.ToUserId).SendAsync("ReceiveNotification", toCount);

                int fromCount = _notifs.GetUnreadUserNotificationsCount(trade.FromUserId);
                await Clients.User(trade.FromUserId).SendAsync("ReceiveNotification", fromCount);
            }
        }

        public async Task SendNewCopyNotifications(string userId, string userName, int copyId, string BookTitle, List<string> pretenders)
        {

            foreach (var pretender in pretenders)
            {
                await SendNewCopyNotification(userId, userName, copyId, BookTitle, pretender);
            }
        }

        public async Task SendNewCopyNotification(string userId, string userName, int copyId, string BookTitle, string pretenderId)
        {
            ApplicationUser fromUser = await userManager.FindByIdAsync(userId);
            if (fromUser == null)
            {
                return;
            }
            ApplicationUser toUser = await userManager.FindByIdAsync(pretenderId);
            if (toUser == null)
            {
                return;
            }

            Notification notification = new Notification
            {
                Type = NotiType.newCopy,
                FromUserId = userId,
                FromUserName = fromUser.UserName,
                FromUserLogin = fromUser.Email,
                ToUserId = pretenderId,
                ToUserName = toUser.UserName,
                ToUserLogin = toUser.Email,
                FromCopyId = copyId,
                FromCopyTitle = BookTitle,
                FromStatus = NotiStatus.read
            };

            if (_notifs.Create(notification) == 1)
            {
                int toCount = _notifs.GetUnreadUserNotificationsCount(pretenderId);
                await Clients.User(pretenderId).SendAsync("ReceiveNotification", toCount);
            }
        }
    }
}
