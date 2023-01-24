namespace ReBook.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Cloudinary _cloudinary;

        public AdministrationService(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            Cloudinary cloudinary)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _cloudinary = cloudinary;
        }

        public UserIndexVM Users(QueryUserVM search)
        {
            try
            {
                UserIndexVM list = new UserIndexVM();

                switch (search.SearchContext)
                {
                    case "index":
                        list.Users = _userManager.Users
                            .OrderBy(u => u.UserName)
                            .Skip(search.CurrentPage * search.PageSize)
                            .Take(search.PageSize)
                            .Select(u => new UserPartialVM() { Id = u.Id, Pseudo = u.UserName, Login = u.Email, Avatar = u.Avatar })
                            .ToList();
                        break;
                    default:

                        if (search.Role != null)
                        {
                            list.Users = _userManager.GetUsersInRoleAsync(search.Role).Result
                                            .Where(u => (
                                                        (search.Id == null || u.Equals(search.Id)) &&
                                                        (search.Pseudo == null || u.UserName.Contains(search.Pseudo)) &&
                                                        (search.Login == null || u.Email.Contains(search.Login))))
                                                .OrderBy(u => u.UserName)
                                                .Skip(search.CurrentPage * search.PageSize)
                                                .Take(search.PageSize)
                                                .Select(u => new UserPartialVM() { Id = u.Id, Pseudo = u.UserName, Login = u.Email, Avatar = u.Avatar })
                                                .ToList();
                        }
                        else
                        {
                            list.Users = _userManager.Users
                                            .Where(u => (
                                                        (search.Id == null || u.Equals(search.Id)) &&
                                                        (search.Pseudo == null || u.UserName.Contains(search.Pseudo)) &&
                                                        (search.Login == null || u.Email.Contains(search.Login))))
                                                .OrderBy(u => u.UserName)
                                                .Skip(search.CurrentPage * search.PageSize)
                                                .Take(search.PageSize)
                                                .Select(u => new UserPartialVM() { Id = u.Id, Pseudo = u.UserName, Login = u.Email, Avatar = u.Avatar })
                                                .ToList();
                        }

                        break;
                }

                search.CurrentPage++;
                search.Found = list.Users.Count;
                list.Query = (QueryUserVM)search.Clone();

                return list;
            }
            catch (Exception e)
            {
                return new UserIndexVM { Error = e.Message };
            }
        }

        public List<IdentityRole> Roles()
        {
            return _roleManager.Roles.OrderBy(x => x.Name).ToList();
        }

        public async Task<string> RoleAdd(string roleName)
        {
            try
            {
                IdentityRole identityRole = new()
                {
                    Name = roleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (!result.Succeeded)
                {
                    var errors = "";

                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }

                    return errors;
                }
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> RoleUpdate(IdentityRole editedRole)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(editedRole.Id);

                if (role == null)
                {
                    return $" Role '{editedRole.Name}' not found.\n";
                }

                role.Name = editedRole.Name;

                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                {
                    var errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }
                    return errors;
                }

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public async Task<string> RoleDelete(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    return $" Role '{roleName}' not found.\n";
                }

                var result = await _roleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    var errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors += error.Description + "\n";
                    }
                    return errors;

                }

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public async Task<IdentityRole> RoleByName(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<string> UserAddRole(string userId, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return $"Role '{roleName}' not found";
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return $"User with Id = {userId} cannot be found";
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                return errors;
            }

            return "OK";
        }

        public async Task<string> UserRemoveRole(string userId, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return $"Role '{roleName}' not found";
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return $"User with Id = {userId} cannot be found";
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                return errors;
            }

            return "OK";
        }

        public async Task<UsersInRoleVM> UsersInRole(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    return new UsersInRoleVM { Error = $"Role '{roleName}' not found" };
                }

                UsersInRoleVM list = new UsersInRoleVM { RoleName = roleName, PageTitle = roleName + "s" };

                var allUsers = _userManager.Users.OrderBy(u => u.UserName).ToList();

                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, roleName))
                    {
                        list.Users.Add(new UserRoleVM
                        {
                            Id = user.Id,
                            Pseudo = user.UserName,
                            Login = user.Email,
                            Avatar = user.Avatar,
                            IsSelected = true
                        });
                    }
                    else
                    {
                        list.Users.Add(new UserRoleVM
                        {
                            Id = user.Id,
                            Pseudo = user.UserName,
                            Login = user.Email,
                            Avatar = user.Avatar,
                            IsSelected = false
                        });
                    }
                }

                return list;
            }
            catch (Exception e)
            {
                return new UsersInRoleVM { Error = e.Message, PageTitle = "unknown error" };
            }
        }
        public async Task<string> SaveAvatar(string fileName, IFormFile file, string UserId)
        {
            if (file == null) return null;

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, stream),
                    PublicId = fileName
                };

                try
                {
                    var uploadResult = _cloudinary.Upload(uploadParams);

                    var user = await _userManager.FindByIdAsync(UserId);

                    if (user != null)
                    {
                        user.Avatar = uploadResult.SecureUrl.AbsoluteUri;
                        try
                        {
                            await _userManager.UpdateAsync(user);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    return uploadResult.SecureUrl.AbsoluteUri;
                }
                catch
                {
                    return null;
                }
            };
        }

        public async Task<UserVM> UserCreate(UserVM obj)
        {
            try
            {
                var user = new ApplicationUser { UserName = obj.Pseudo, Email = obj.Email, Avatar = obj.Avatar, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, obj.Password);

                if (result.Succeeded)
                {
                    obj.Id = user.Id;

                    if (obj.Roles.Count > 0)
                    {
                        foreach (var role in obj.Roles)
                        {
                            var newRole = role.Trim();

                            if (await _roleManager.FindByNameAsync(newRole) == null)
                            {
                                IdentityRole identityRole = new()
                                {
                                    Name = newRole
                                };
                                IdentityResult result2 = await _roleManager.CreateAsync(identityRole);
                            }

                            await _userManager.AddToRoleAsync(user, newRole);
                        }

                        obj.Roles.Clear();

                        var rolesForUser = await _userManager.GetRolesAsync(user);

                        if (rolesForUser.Count > 0)
                        {
                            foreach (var role in rolesForUser.ToList())
                            {
                                obj.Roles.Add(role);
                            }
                        }
                    }
                    return obj;
                }

                obj.Error = OutputIdentityErrors(result);
                return obj;
            }
            catch (Exception e)
            {
                obj.Error = e.Message;
                return obj;
            }
        }

        public async Task<UserVM> UserUpdate(UserVM obj)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(obj.Id);

                if (user == null)
                {
                    return null;
                }

                user.UserName = obj.Pseudo;
                user.Email = obj.Email;
                user.Avatar = obj.Avatar;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    obj.Error = OutputIdentityErrors(result);
                }

                var rolesForUser = await _userManager.GetRolesAsync(user);

                if (rolesForUser.Count > 0)
                {
                    foreach (var role in rolesForUser.ToList())
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                if (obj.Roles.Count > 0)
                {
                    foreach (var role in obj.Roles)
                    {
                        var newRole = role.Trim();

                        if (await _roleManager.FindByNameAsync(newRole) == null)
                        {
                            IdentityRole identityRole = new()
                            {
                                Name = newRole
                            };
                            IdentityResult result2 = await _roleManager.CreateAsync(identityRole);
                        }

                        await _userManager.AddToRoleAsync(user, newRole);
                    }

                    obj.Roles.Clear();

                    rolesForUser = await _userManager.GetRolesAsync(user);

                    if (rolesForUser.Count > 0)
                    {
                        foreach (var role in rolesForUser.ToList())
                        {
                            obj.Roles.Add(role);
                        }
                    }
                }

                return obj;
            }
            catch (Exception e)
            {
                obj.Error = e.Message;
                return obj;
            }
        }
        private string OutputIdentityErrors(IdentityResult result)
        {
            string errorMessage = "";
            foreach (var err in result.Errors)
            {
                errorMessage += err.Description + "/n";
            }

            return errorMessage;
        }

        public async Task<UserVM> UserGetById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return null;
                }

                var rolesForUser = await _userManager.GetRolesAsync(user);

                var roleList = new List<string>();

                if (rolesForUser.Count > 0)
                {
                    foreach (var role in rolesForUser.ToList())
                    {
                        roleList.Add(role);
                    }
                }

                return new UserVM
                {
                    Id = user.Id,
                    Pseudo = user.UserName,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Roles = roleList
                };
            }
            catch (Exception e)
            {
                return new UserVM
                {
                    Error = e.Message
                };
            }
        }

        public async Task<string> UserDelete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return $"User with id '{id}' not found";
                }
                var rolesForUser = await _userManager.GetRolesAsync(user);

                if (rolesForUser.Count > 0)
                {
                    foreach (var role in rolesForUser.ToList())
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                await _userManager.DeleteAsync(user);
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
