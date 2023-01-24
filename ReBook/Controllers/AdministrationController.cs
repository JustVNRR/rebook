namespace ReBook.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IAdministrationService _admin;
        private readonly IJsonRenderService _json;

        public AdministrationController(
            IAdministrationService admin,
            IJsonRenderService viewRenderService)
        {
            _json = viewRenderService;
            _admin = admin;
        }

        public async Task<string> AddOrRemoveUserRole(string roleName, string userId, string action)
        {
            string output = "";
            string result = null;
            if (action == "add")
            {
                result = await _admin.UserAddRole(userId, roleName);
                output = $"<span class='text-success'> is now '{roleName}' </span>";
            }
            else if (action == "remove")
            {
                result = await _admin.UserRemoveRole(userId, roleName);
                output = $"<span class='text-success'> is no longer '{roleName}' </span>";
            }

            if (result != "OK")
            {
                return $"<span class='text-danger'>{result}</span>";
            }
            return output;
        }

        [HttpGet]
        public async Task<IActionResult> _Roles()
        {
            return await _json.RenderAsync("Administration/_IndexRoles", _admin.Roles());
        }

        [ServiceFilter(typeof(AuthorisationFilter))]
        public async Task<IActionResult> _AddRoleGet()
        {
            return await _json.RenderAsync("Administration/_AddRole", new RoleVM { PageTitle = "New Role" });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> _AddRole(RoleVM newRole)
        {
            if (!ModelState.IsValid)
            {
                newRole.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Administration/_AddRole", newRole);
            }

            var result = await _admin.RoleAdd(newRole.Name);

            if (result != "OK")
            {
                newRole.Error = result;

                return await _json.RenderAsync("Administration/_AddRole", newRole);
            }

            return await _json.RenderAsync("Administration/_IndexRoles", _admin.Roles());
        }

        [HttpGet]
        public async Task<ActionResult> _DeleteRoleGet(string roleName)
        {
            var role = await _admin.RoleByName(roleName);

            if (role == null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Delete Role",
                    Message = $"Role {roleName} not found",
                    Error = true
                };

                return await _json.RenderAsync("_MessageBox", message);
            }

            return await _json.RenderAsync("Administration/_DeleteRole", new RoleVM { PageTitle = "Delete Role", Name = roleName });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> _DeleteRole(RoleVM roleVM)
        {
            if (!ModelState.IsValid)
            {
                roleVM.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Administration/_DeleteRole", roleVM);
            }

            var result = await _admin.RoleDelete(roleVM.Name);

            if (result != "OK")
            {
                roleVM.Error = result;

                return await _json.RenderAsync("Administration/_DeleteRole", roleVM);
            }

            return await _json.RenderAsync("Administration/_IndexRoles", _admin.Roles());
        }

        [HttpGet]
        public async Task<ActionResult> _EditRoleGet(string roleName)
        {
            var role = await _admin.RoleByName(roleName);

            if (role == null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Edit Role",
                    Message = $"Role {roleName} not found",
                    Error = true
                };

                return await _json.RenderAsync("_MessageBox", message);
            }

            return await _json.RenderAsync("Administration/_EditRole", new RoleVM { PageTitle = "Edit Role", Name = roleName, Id = role.Id });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> _EditRole(RoleVM roleVM)
        {
            if (!ModelState.IsValid)
            {
                roleVM.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Administration/_EditRole", roleVM);
            }

            var result = await _admin.RoleUpdate(new IdentityRole { Id = roleVM.Id, Name = roleVM.Name });

            if (result != "OK")
            {
                roleVM.Error = result;

                return await _json.RenderAsync("Administration/_EditRole", roleVM);
            }

            return await _json.RenderAsync("Administration/_IndexRoles", _admin.Roles());
        }

        [HttpGet]
        public async Task<ActionResult> _UsersInRoleGet(string RoleName)
        {
            return await _json.RenderAsync("Administration/_UsersInRole", await _admin.UsersInRole(RoleName.Trim()));
        }

        [HttpGet]
        // [ServiceFilter(typeof(AuthorisationFilter))]
        public IActionResult Users()
        {
            return View(new UserIndexVM());
        }


        [HttpGet]
        public async Task<IActionResult> _UsersIndexPartial(QueryUserVM query)
        {
            if (ModelState.IsValid)
            {
                return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" },
                                                                   "Administration/_UsersIndexPartial",
                                                                   _admin.Users(query));
            }

            var message = new MessageBoxVM
            {
                Title = "Invalid Form",
                Message = OutputModelState(ModelState),
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpGet]
        public async Task<IActionResult> _UserAddGet()
        {
            return await _json.RenderAsync("Administration/_UserCreateOrEdit", new UserVM());
        }

        [HttpGet]
        public async Task<IActionResult> _UserEditGet(string Id)
        {
            UserVM userVM = await _admin.UserGetById(Id);

            if (userVM != null)
            {
                userVM.PageTitle = "User Update";
                return await _json.RenderAsync("Administration/_UserCreateOrEdit", userVM);
            }
            var message = new MessageBoxVM
            {
                Title = "User Update",
                Message = "User not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _UserAddOrEditPost(UserVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Administration/_UserCreateOrEdit", obj);
            }

            obj.PageTitle = "User Details";

            if (obj.Id == null)
            {
                obj = await _admin.UserCreate(obj);
            }
            else
            {
                obj = await _admin.UserUpdate(obj);
            }

            return await _json.RenderAsync(new JsonVM { callback = "_UsersIndexPartial" }, "Administration/_UserDetails", obj);

            //Mystere...
            //return await _json.RenderAsync(new JsonVM { callback = "Administration/_UsersIndexPartial" }, "Administration/_UserDetails", obj);
        }


        public async Task<IActionResult> _AddRoleInForm(UserVM obj)
        {
            obj.Roles.Add("");
            return await _json.RenderAsync(new JsonVM { targetId = "#rolesListContainer" }, "Administration/_AddRolePartial", obj);
        }

        public async Task<IActionResult> _UpdateRolesInForm(UserVM obj)
        {
            return await _json.RenderAsync(new JsonVM { targetId = "#rolesListContainer" }, "Administration/_AddRolePartial", obj);
        }

        [HttpGet]
        public async Task<IActionResult> _UserDetails(string id)
        {
            UserVM userVM = await _admin.UserGetById(id);

            if (userVM != null)
            {
                userVM.PageTitle = "User Details";
                return await _json.RenderAsync("Administration/_UserDetails", userVM);
            }
            var message = new MessageBoxVM
            {
                Title = "User Details",
                Message = "User not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpGet]
        public async Task<IActionResult> _UserDeleteGet(string id)
        {
            UserVM userVM = await _admin.UserGetById(id);

            if (userVM != null)
            {
                userVM.PageTitle = "Delete User";
                return await _json.RenderAsync("Administration/_UserDelete", userVM);
            }
            var message = new MessageBoxVM
            {
                Title = "USer Details",
                Message = "User not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _UserDeletePost(string id)
        {
            var message = new MessageBoxVM
            {
                Title = "User Delete"
            };

            var result = await _admin.UserDelete(id);

            if (result == "OK")
            {
                message.Message = "User successfully deleted";
                //return await _json.RenderAsync(new JsonVM { callback = "Administration/_UsersIndexPartial" }, "_MessageBox", message);
                //Mystere...
                return await _json.RenderAsync(new JsonVM { callback = "_UsersIndexPartial" }, "_MessageBox", message);
            }
            else
            {
                message.Message = result;
                message.Error = true;
                return await _json.RenderAsync("_MessageBox", message);
            }
        }

        public async Task<IActionResult> _FindByContextLink(string name, string context)
        {
            QueryUserVM query = new QueryUserVM() { SearchContext = context };

            if (context == "role") query.Role = name;
            else if (context == "login") query.Login = name;
            else if (context == "pseudo") query.Pseudo = name;
            else if (context == "id") query.Id = name;

            return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" },
                                                                    "Administration/_UsersIndexPartial",
                                                                    _admin.Users(query));
        }
        private string OutputModelState(ModelStateDictionary modelState)
        {
            string errors = "";
            errors += string.Join("/n", modelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
            return errors;
        }

        [HttpPost]
        public async Task<string> SaveAvatar(IFormCollection fileName, IFormFile file, string userId)
        {
            return await _admin.SaveAvatar(fileName["name"], file, userId);
        }
    }
}
