namespace ReBook.Interfaces.Services
{
    public interface ICopyService
    {
        Copy GetById(int id);
        Copy Add(Copy copy);
        int Delete(int id);
        Edition GetEditionById(int editionId);
        CopyIndexVM Get(QueryVM search);
        bool IsCurrentUserLookingForRelatetdEdition(string id, int editionId);
        Copy Update(Copy copy);
        AvailableCopiesVM GetAvailableCopies(int editionId, string userId);
        int WishCopieAdd(int copyId, string userId);
        int WishCopieRemove(int copyId, string userId);
        List<Copy> GetAllAvailablesByOwnerId(string userId, int skipRows, int pageSize);
        int FindMatch(string ownerId, string currentUserId);
        List<string> FindEditionPretenders(int editionId);
    }
}
