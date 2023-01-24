namespace ReBook.Interfaces.Services
{
    public interface IWishService
    {
        public Wish Add(Wish wish);
        public int Delete(Wish wish);
        public EditionIndexVM Get(QueryVM search);
    }
}
