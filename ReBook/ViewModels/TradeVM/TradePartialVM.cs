namespace ReBook.ViewModels.TradeVM
{
    public class TradePartialVM
    {
        public int Id { get; set; }
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string FromBookTitle { get; set; }
        public string ToBookTitle { get; set; }
        public int FromCopyId { get; set; }
        public int ToCopyId { get; set; }
        public string FromCopyVisual { get; set; } = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
        public string ToCopyVisual { get; set; } = "https://res.cloudinary.com/dngz4sjc2/image/upload/v1624033702/Editions/book-placeholder.jpg";
        public TradeStatus Status { get; set; }
        public TradeStatus FromStatus { get; set; }
        public TradeStatus ToStatus { get; set; }
    }
}
