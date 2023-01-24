namespace ReBook.Interfaces.Services
{
    public interface ITradeService
    {
        TradeIndexVM Get(QueryTradeVM search);

        int Add(Trade trade);

        Trade GetById(int id);

        Trade Update(Trade trade);
    }
}
