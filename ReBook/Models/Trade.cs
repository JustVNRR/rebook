namespace ReBook.Models
{
    public enum TradeStatus
    {
        pending, complete
    }

    public class Trade
    {
        [Key]
        public int Id { get; set; } = 0;
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserLogin { get; set; }
        public string FromUserAddress { get; set; }
        public string FromUserComment { get; set; }
        public float FromUserRating { get; set; }
        public int FromCopyId { get; set; }
        public string FromCopyVisual { get; set; }
        public string FromCopyTitle { get; set; }

        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ToUserLogin { get; set; }
        public string ToUserAddress { get; set; }
        public string ToUserComment { get; set; }
        public float ToUserRating { get; set; }
        public int ToCopyId { get; set; }
        public string ToCopyVisual { get; set; }
        public string ToCopyTitle { get; set; }

        public TradeStatus Status { get; set; } = TradeStatus.pending;
        public TradeStatus FromStatus { get; set; } = TradeStatus.pending;
        public TradeStatus ToStatus { get; set; } = TradeStatus.pending;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedDateSt => this.CreatedDate.ToString("dd-MMM-yyyy HH:mm:ss");
    }
}
