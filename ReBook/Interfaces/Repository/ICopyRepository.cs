namespace ReBook.Interfaces.Repository
{
    public interface ICopyRepository
    {
        // Copy GetById(int id);
        //List<CopyPartialVM> GetAll(int offset, int count);
        //public List<CopyPartialVM> GetAdvanced(QueryVM search);

        Copy Insert(Copy copy);
        Copy Update(Copy copy);
        int DeleteById(int id);

        public Copy GetById(int id);

        Edition GetEditionById(int editionId);
        List<CopyPartialVM> GetAllPartialByOwnerId(QueryVM search);

        List<CopyPartialVM> GetPartialByOwnerId(QueryVM search);
        List<CopyPartialVM> GetPartialAdvancedByOwnerId(QueryVM search);
        List<Copy> GetAllAvailablesByOwnerId(string ownerId, int skipRows, int pageSize);
        bool IsCurrentUserLookingForRelatetdEdition(string id, int editionId);
        AvailableCopiesVM GetAvailableCopies(int editionId, string userId);
        int WishCopieAdd(int copyId, string userId);
        int WishCopieRemove(int copyId, string userId);
        int FindMatch(string ownerId, string currentUserId);
        List<string> FindEditionPretenders(int editionId);

        //string SaveVisuals(string fileName, IFormFile file);
    }
}
