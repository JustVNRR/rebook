namespace ReBook.Interfaces.Repository
{
    public interface INotificationRepository
    {
        List<Notification> GetUserNotifications(string userId, bool unreadOnly);
        int GetUnreadUserNotificationsCount(string userId);
        int Create(Notification notification);
        void ReadNotification(int notificationId, string userId);
        Notification GetById(int notificationId);
        int Update(Notification notification);
    }
}
