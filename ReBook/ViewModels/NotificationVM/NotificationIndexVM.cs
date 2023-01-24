namespace ReBook.ViewModels.NotificationVM
{
    public class NotificationIndexVM
    {
        public string PageTitle { get; set; }

        public List<Notification> Notifications { get; set; }

        public NotificationIndexVM()
        {
            PageTitle = "Notifications";
        }
    }
}
