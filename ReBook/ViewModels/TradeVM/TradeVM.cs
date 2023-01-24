namespace ReBook.ViewModels.TradeVM
{
    public class TradeVM
    {
        public string PageTitle { get; set; } = "Trade update";
        public string Error { get; set; }
        public int TradeId { get; set; }
        public string UserId { get; set; }
        public string Book { get; set; }
        public string Comment { get; set; }

        [BindProperty]
        public float Rating { get; set; } = 0f;
        public int[] Ratings { get; set; } = new[] { 1, 2, 3, 4, 5 };

        [BindProperty]
        public bool Received { get; set; } = false;
        public bool[] Receiveds { get; set; } = new[] { false, true };
    }
}
