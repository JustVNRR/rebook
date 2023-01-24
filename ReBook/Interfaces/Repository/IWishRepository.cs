namespace ReBook.Interfaces.Repository
{
    public interface IWishRepository
    {
        Wish Add(Wish wish);
        int Delete(Wish wish);
        List<EditionPartialVM> GetAllByUserId(string userId, int skipRows, int pageSize);
        List<EditionPartialVM> GetByUserId(string userId, string query, int skipRows, int pageSize);
        List<EditionPartialVM> GetAdvancedByUserId(QueryVM search);
    }
}
