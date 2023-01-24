namespace ReBook.Interfaces.Services
{
    public interface IEditionService
    {
        Edition GetByISBN(string ISBN);
        Edition GetById(int id);

        EditionIndexVM Get(QueryVM search);
       
        int Delete(int id);

        EditionIndexVM GetWished(QueryVM search);

        EditionIndexVM GetAvailables(QueryVM search);

        Edition Create(Edition edition);

        Edition Update(Edition edition);

        bool IsUserLookingFor(int editionID, string userId);

        bool IsUserOwnerOf(int editionID, string userId);

        int GetAvailableCopiesCount(int editionId);
        List<string> GetAuthorsList();

        List<string> GetEditorsList();

        List<string> GetBooksList();

        List<string> GetTagsList();

        Wish AddWish(Wish wish);
        string SaveCover(string fileName, IFormFile file);
    }
}
