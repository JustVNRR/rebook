namespace ReBook.Interfaces.Repository
{
    public interface ITradeRepository
    {
        List<TradePartialVM> GetAllPartialByUserId(QueryTradeVM search);
        List<TradePartialVM> GetPartialByUserId(QueryTradeVM search);
        List<TradePartialVM> GetPartialAdvancedByUserId(QueryTradeVM search);
        int Insert(Trade trade);
        Trade GetById(int id);
        Trade Update(Trade trade);
    }
}
