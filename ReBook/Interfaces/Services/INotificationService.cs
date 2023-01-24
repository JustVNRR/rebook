namespace ReBook.Interfaces.Services
{
    public interface INotificationService
    {
        int Create(Notification notification);

        List<Notification> GetUserNotifications(string userId, bool unreadOnly);

        void ReadNotification(int notificationId, string userId);

        int GetUnreadNumber(string userId);
        Notification GetById(int notificationId);

        int Update(Notification notification);
    }
}
