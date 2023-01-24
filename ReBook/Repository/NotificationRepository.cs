namespace ReBook.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        public ReBookDbContext _db { get; }

        public NotificationRepository(ReBookDbContext db)
        {
            _db = db;
        }

        public int Create(Notification notification)
        {
            _db.Notifications.Add(notification);
            return _db.SaveChanges();
        }

        public List<Notification> GetUserNotifications(string userId, bool unreadOnly)
        {
            if (unreadOnly)
            {
                return _db.Notifications
                                        .Where(u => ((u.Status == NotiStatus.unread) &&
                                                        ((u.ToUserId.Equals(userId) && u.ToStatus == NotiStatus.unread) ||
                                                        (u.FromUserId.Equals(userId) && u.FromStatus == NotiStatus.unread))))
                                        .OrderByDescending(u => u.CreatedDate)
                                        .ToList();
            }

            return _db.Notifications.Where(u => (u.ToUserId.Equals(userId) || (u.FromUserId.Equals(userId)))).OrderByDescending(u => u.CreatedDate).ToList();
        }

        public Notification GetById(int notificationId)
        {
            return _db.Notifications.Where(n => n.Id == notificationId).FirstOrDefault();
        }

        public int Update(Notification notification)
        {
            _db.Notifications.Update(notification);
            return _db.SaveChanges();
        }

        public int GetUnreadUserNotificationsCount(string userId)
        {
            var list = _db.Notifications.Where(u => ((u.Status == NotiStatus.unread) &&
                                                        ((u.ToUserId.Equals(userId) && u.ToStatus == NotiStatus.unread) ||
                                                        (u.FromUserId.Equals(userId) && u.FromStatus == NotiStatus.unread)))).ToList();

            if (list != null)
            {
                return list.Count;
            }
            else
            {
                return 0;
            }
        }

        public void ReadNotification(int notiId, string userId)
        {
            var notification = _db.Notifications.FirstOrDefault(n => (n.ToUserId.Equals(userId) || n.FromUserId.Equals(userId)) && n.Id == notiId);

            if (notification == null) return;

            if (userId == notification.FromUserId)
            {
                notification.FromStatus = NotiStatus.read;
            }
            else if (userId == notification.ToUserId)
            {
                notification.ToStatus = NotiStatus.read;
            }
            _db.Notifications.Update(notification);
            _db.SaveChanges();
        }
    }
}