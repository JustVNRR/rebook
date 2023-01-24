namespace ReBook.Interfaces.Services
{
    public interface IEditionAPIService
    {
        Task<Edition> GetByISBN(string ISBN);
        IEnumerable<Edition> Get(string query, int offset, int count);
        Task<EditionIndexVM> GetAdvanced(QueryVM search);

        Task<EditionPartialVM> GetByISBNPartial(string ISBN);
        List<EditionPartialVM> GetPartial(string query, int offset, int count);
        /*Task<List<EditionPartialVM>> GetAdvancedPartial(QueryVM search);*/
    }
}
