namespace ReBook.ViewModels.TradeVM
{

    public enum TradeStatusVM
    {
        ALL, COMPLETE, WAITING, TOSEND
    }

    public class QueryTradeVM
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int Found { get; set; }
        public string SearchContext { get; set; }
        public string QuerySearch { get; set; }
        public string Title { get; set; }

        public string UserId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }

        [BindProperty]
        public TradeStatus Status { get; set; } = TradeStatus.pending;
        public TradeStatus[] Statuss { get; set; }

        [BindProperty]
        public TradeStatusVM StatusVM { get; set; } = TradeStatusVM.ALL;
        public TradeStatusVM[] StatusVMs { get; set; }

        public TradeStatus FromStatus { get; set; } = TradeStatus.pending;
        public TradeStatus ToStatus { get; set; } = TradeStatus.pending;
        public string CurrentController { get; set; }


        public QueryTradeVM()
        {
            SearchContext = "index";
            QuerySearch = null;
            Title = null;
            PageSize = 5;
            CurrentPage = 0;
            Found = 0;
            FromUserId = null;
            ToUserId = null;
            CurrentController = "trades";
            Statuss = new[] { TradeStatus.complete, TradeStatus.pending };
            StatusVMs = new[] { TradeStatusVM.ALL, TradeStatusVM.COMPLETE, TradeStatusVM.WAITING, TradeStatusVM.TOSEND };
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
