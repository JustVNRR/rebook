namespace ReBook.Interfaces.Services
{
    public interface IAdministrationService
    {
        UserIndexVM Users(QueryUserVM search);
        List<IdentityRole> Roles();
        Task<string> RoleAdd(string roleName);
        Task<string> RoleUpdate(IdentityRole editedRole);
        Task<string> RoleDelete(string roleName);
        Task<IdentityRole> RoleByName(string roleName);
        Task<string> UserAddRole(string userId, string roleName);
        Task<string> UserRemoveRole(string userId, string roleName);
        Task<UsersInRoleVM> UsersInRole(string roleName);
        Task<string> SaveAvatar(string fileName, IFormFile file, string UserId);
        Task<UserVM> UserCreate(UserVM obj);
        Task<UserVM> UserUpdate(UserVM obj);
        Task<UserVM> UserGetById(string id);
        Task<string> UserDelete(string id);
    }
}
