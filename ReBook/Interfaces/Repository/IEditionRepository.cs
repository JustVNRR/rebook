namespace ReBook.Interfaces.Repository
{
    public interface IEditionRepository
    {
        Edition GetById(int id);
        Edition GetByISBN(string ISBN);
        List<EditionPartialVM> Get(QueryVM search);
        List<EditionPartialVM> GetWished(QueryVM query);
        List<EditionPartialVM> GetAvailables(QueryVM search);
        List<EditionPartialVM> GetAll(QueryVM search);
        List<EditionPartialVM> GetAllWished(QueryVM search);
        List<EditionPartialVM> GetAllAvailables(QueryVM search);
        List<EditionPartialVM> GetAdvanced(QueryVM search);
        List<EditionPartialVM> GetAdvancedWished(QueryVM search);
        List<EditionPartialVM> GetAdvancedAvailables(QueryVM search);
        Edition Insert(Edition edition);
        Edition Update(Edition edition);
        int DeleteById(int id);
        List<string> GetAuthorsList();
        List<string> GetEditorsList();
        List<string> GetBooksList();
        List<string> GetTagsList();
        bool IsUserLookingFor(int editionID, string userId);
        bool IsUserOwnerOf(int editionID, string userId);
        Wish AddWish(Wish wish);

        string SaveCover(string fileName, IFormFile file);
        int GetAvailableCopiesCount(int editionId);

    }
}
