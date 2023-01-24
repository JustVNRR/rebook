namespace ReBook.Models
{
    public enum NotiStatus
    {
        unread, read, accepted, refused, toolate
    }

    public enum NotiType
    {
        match, accepted, newtrade, updateTrade, wish, refused, newCopy
    }

    public class Notification
    {
        [Key]
        public int Id { get; set; } = 0;
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserLogin { get; set; }
        public string FromUserAddress { get; set; }
        public int FromCopyId { get; set; }
        public string FromCopyVisual { get; set; }
        public string FromCopyTitle { get; set; }

        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ToUserLogin { get; set; }
        public string ToUserAddress { get; set; }
        public int ToCopyId { get; set; }
        public string ToCopyVisual { get; set; }
        public string ToCopyTitle { get; set; }

        public float UserRatings { get; set; }
        public string UserComment { get; set; } = "";

        public bool UserReceived { get; set; } = false;
        public NotiStatus Status { get; set; } = NotiStatus.unread;
        public NotiStatus FromStatus { get; set; } = NotiStatus.unread;
        public NotiStatus ToStatus { get; set; } = NotiStatus.unread;

        public NotiType Type { get; set; } = NotiType.match;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedDateSt => this.CreatedDate.ToString("dd-MMM-yyyy HH:mm:ss");
    }
}