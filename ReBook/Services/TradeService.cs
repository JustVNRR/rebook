namespace ReBook.Services
{
    public class TradeService :ITradeService
    {
        public ITradeRepository _dao;

        public TradeService(ITradeRepository dao)
        {
            _dao = dao;
        }

        public TradeIndexVM Get(QueryTradeVM search)
        {
            if (String.IsNullOrEmpty(search.QuerySearch))
            {
                search.SearchContext = "index";
            }
            else
            {
                search.QuerySearch = search.QuerySearch.Trim();
            }

            TradeIndexVM list = new TradeIndexVM();

            switch (search.StatusVM)
            {
                case TradeStatusVM.ALL:
                    break;
                case TradeStatusVM.COMPLETE:
                    search.FromStatus = TradeStatus.complete;
                    search.ToStatus = TradeStatus.complete;
                    search.Status = TradeStatus.complete;
                    break;
                case TradeStatusVM.WAITING:
                    search.FromStatus = TradeStatus.complete;
                    search.ToStatus = TradeStatus.complete;
                    search.Status = TradeStatus.pending;
                    break;
                case TradeStatusVM.TOSEND:
                    search.FromStatus = TradeStatus.pending;
                    search.ToStatus = TradeStatus.pending;
                    search.Status = TradeStatus.pending;
                    break;
                default:
                    break;
            }


            switch (search.SearchContext)
            {
                case "index":
                    list.Trades = _dao.GetAllPartialByUserId(search);
                    break;
                case "simple":
                    if (!String.IsNullOrEmpty(search.QuerySearch))
                        list.Trades = _dao.GetPartialByUserId(search);
                    break;
                default:
                    list.Trades = _dao.GetPartialAdvancedByUserId(search);
                    break;
            }

            search.CurrentPage++;
            search.Found = list.Trades.Count;
            list.Query = (QueryTradeVM)search.Clone();

            return list;
        }

        public int Add(Trade trade)
        {
            return _dao.Insert(trade);
        }

        public Trade GetById(int id)
        {
            return _dao.GetById(id);
        }

        public Trade Update(Trade trade)
        {
            return _dao.Update(trade);
        }
    }
}
