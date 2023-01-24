namespace ReBook.Services
{
    public class NotificationService : INotificationService
    {
        public INotificationRepository _dao;

        public NotificationService(INotificationRepository dao)
        {
            _dao = dao;
        }

        public int Create(Notification notification)
        {
            return _dao.Create(notification);
        }

        public List<Notification> GetUserNotifications(string userId, bool unreadOnly)
        {
            return _dao.GetUserNotifications(userId, unreadOnly);
        }

        public void ReadNotification(int notificationId, string userId)
        {
            _dao.ReadNotification(notificationId, userId);
        }

        public int GetUnreadNumber(string userId)
        {
            return _dao.GetUnreadUserNotificationsCount(userId);
        }
        public Notification GetById(int notificationId)
        {
            return _dao.GetById(notificationId);
        }

        public int Update(Notification notification)
        {
            return _dao.Update(notification);
        }
    }
}
