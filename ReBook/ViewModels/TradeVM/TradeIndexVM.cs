namespace ReBook.ViewModels.TradeVM
{
    public class TradeIndexVM
    {
        public List<TradePartialVM> Trades { get; set; }
        public QueryTradeVM Query { get; set; }
        public string CurrentController { get; set; }

        public TradeIndexVM()
        {
            Trades = new List<TradePartialVM>();
            Query = new QueryTradeVM();
            CurrentController = "trades";
        }
    }
}
