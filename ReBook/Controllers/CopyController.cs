namespace ReBook.Controllers
{
    [LoginFilter]
    public class CopyController : Controller
    {

        private readonly IJsonRenderService _json;
        private readonly ICopyService _copies;
        private readonly UserManager<ApplicationUser> _userManager;

        public CopyController(
                                ICopyService copies,
                                UserManager<ApplicationUser> userManager,
                                IJsonRenderService viewRenderService,
                                Cloudinary cloudinary)
        {
            _userManager = userManager;
            _copies = copies;
            _json = viewRenderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(_copies.Get(new QueryVM { OwnerId = user.Id, CurrentController = "copies", Repo = "copies" }));
        }

        [HttpGet]
        public async Task<IActionResult> _IndexPartial(QueryVM query)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                query.OwnerId = user.Id;
                query.CurrentController = "copies";

                return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Copy/_IndexPartial", _copies.Get(query));
            }

            var message = new MessageBoxVM
            {
                Title = "Invalid Form",
                Message = OutputModelState(ModelState),
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        public async Task<IActionResult> _AddGet(string userId, int editionId, string currentController)
        {
            Edition edition = _copies.GetEditionById(editionId);

            if (edition == null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Edition with id '{editionId}' Not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }
            else if (user.Id != userId)
            {
                var message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = $"User with id '{userId}' is not current logged in user.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            CopyVM copyVM = new CopyVM();
            copyVM.PageTitle = "My Books";
            copyVM.CurrentController = currentController;

            copyVM.Copy.Edition = edition;
            copyVM.Copy.EditionId = edition.Id;

            copyVM.Copy.Book = edition.Book;
            copyVM.Copy.BookId = edition.Book.Id;

            //copyVM.Copy.Owner = user;
            copyVM.Copy.OwnerId = userId;

            return await _json.RenderAsync("Copy/_CreateOrEditCopy", copyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateOrEditCopyPost(CopyVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Copy/_CreateOrEditCopy", obj);
            }

            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            obj.Copy.Owner = user;

            Edition edition = _copies.GetEditionById(obj.Copy.EditionId);

            if (edition == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Edition with id '{obj.Copy.EditionId}' Not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            obj.Copy.Edition = edition;
            obj.Copy.BookId = edition.Book.Id;
            obj.Copy.Book = edition.Book;

            if (obj.Copy.Id == 0)
            {
                Copy copy = _copies.Add(obj.Copy);

                if (copy == null)
                {
                    message = new MessageBoxVM
                    {
                        Title = "Unexpected error",
                        Message = "Something went wrong while registering your book.",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }

                message = new MessageBoxVM
                {
                    Title = "",
                    Message = "Your book was successfully registered.",
                };

                List<string> areLookingFor = new List<string>();

                if (copy.Avalaible == true)
                {
                    areLookingFor = _copies.FindEditionPretenders(copy.EditionId);
                }

                if (areLookingFor.Any())
                {
                    var toto = JsonConvert.SerializeObject(areLookingFor);

                    string notif = $"connection.invoke('SendNewCopyNotifications', '{user.Id}','{user.UserName}', {copy.Id}, '{copy.Book.Title}', {toto});";

                    if (obj.CurrentController == "editions")
                    {
                        return await _json.RenderAsync(new JsonVM { notification = notif }, "_MessageBox", message);
                    }
                    else
                    {
                        return await _json.RenderAsync(new JsonVM { callback = "Copy/_IndexPartial", notification = notif }, "_MessageBox", message);
                    }
                }

                if (obj.CurrentController == "copies")
                {
                    return await _json.RenderAsync(new JsonVM { callback = "$('#main-search-button').trigger('click');" }, "_MessageBox", message);
                }
                else
                {
                    return await _json.RenderAsync("_MessageBox", message);
                }
            }
            else
            {
                Copy copy = _copies.Update(obj.Copy);

                if (copy == null)
                {
                    message = new MessageBoxVM
                    {
                        Title = "Unexpected error",
                        Message = "Something went wrong while updating your book.",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }

                obj.Copy = copy;

                List<string> areLookingFor = new List<string>();

                if (copy.Avalaible == true)
                {
                    areLookingFor = _copies.FindEditionPretenders(copy.EditionId);
                }

                if (areLookingFor.Any())
                {
                    var toto = JsonConvert.SerializeObject(areLookingFor);

                    string notif = $"connection.invoke('SendNewCopyNotifications', '{user.Id}','{user.UserName}', {copy.Id}, '{copy.Book.Title}', {toto});";

                    return await _json.RenderAsync(new JsonVM { callback = "$('#main-search-button').trigger('click');", notification = notif }, "Copy/_Details", obj);
                }

                return await _json.RenderAsync(new JsonVM { callback = "$('#main-search-button').trigger('click');" }, "Copy/_Details", obj);
            }
        }

        public async Task<IActionResult> _Details(int id, string currentController)
        {
            MessageBoxVM message;

            Copy copy = _copies.GetById(id);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{id}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            CopyVM copyVM = new CopyVM();

            copyVM.Copy = copy;

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (user.Id == copy.OwnerId)
                {
                    copyVM.IsOwnerOf = true;
                }
                else if (_copies.IsCurrentUserLookingForRelatetdEdition(user.Id, copy.EditionId))
                {
                    copyVM.IsLookingForEdition = true;
                }
            }

            return await _json.RenderAsync("Copy/_Details", copyVM);
        }

        public async Task<IActionResult> _UserIndex(string userId, int initialCopyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var targetUser = await _userManager.FindByIdAsync(userId);

            MessageBoxVM message;

            if (user == null || targetUser == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "User not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            List<Copy> list = _copies.GetAllAvailablesByOwnerId(userId, 0, 10);

            CopyUserIndexVM ownerIndexVM = new CopyUserIndexVM();
            ownerIndexVM.PageTitle = targetUser.UserName + "'s books";
            ownerIndexVM.TargetedUserName = targetUser.UserName;
            ownerIndexVM.TargetedUserId = userId;

            if (list.Any())
            {
                ownerIndexVM.Copies = list;
            }
            ownerIndexVM.CurrentUserId = user.Id;
            ownerIndexVM.CurrentUserName = user.UserName;
            ownerIndexVM.InitialCopyId = initialCopyId;
            ownerIndexVM.Callback = "/Copy/_Details?id=" + initialCopyId + "&&currentController=copies";

            return await _json.RenderAsync("Copy/_OwnerIndex", ownerIndexVM);
        }

        public async Task<IActionResult> _RemoveByEditionId(EditionVM editionVM)
        {
            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var copies = _copies.Get(new QueryVM { OwnerId = user.Id, ISBN = editionVM.Edition.ISBN13, SearchContext = "advanced" });

            foreach (var copy in copies.Copies)
            {
                if (_copies.Delete(copy.Id) == -1)
                {
                    message = new MessageBoxVM
                    {
                        Title = "Unexpected error",
                        Message = "Something went wrong while removing this book from your library.",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }
            }

            message = new MessageBoxVM
            {
                Title = "",
                Message = "This book was removed from your library."
            };

            if (editionVM.CurrentController == "editions")
            {
                return await _json.RenderAsync("_MessageBox", message);
            }
            else
            {
                return await _json.RenderAsync(new JsonVM { callback = "Copy/_IndexPartial" }, "_MessageBox", message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _EditGet(CopyVM obj)
        {
            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            Copy copy = _copies.GetById(obj.Copy.Id);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{obj.Copy.Id}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            if (copy.OwnerId != user.Id)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "You are not the owner of this book.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            obj.Copy = copy;
            obj.PageTitle = "Edit Book";
            obj.IsOwnerOf = true;

            return await _json.RenderAsync("Copy/_CreateOrEditCopy", obj);

        }

        public async Task<IActionResult> _RemoveGet(int copyId, string currentController)
        {
            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            Copy copy = _copies.GetById(copyId);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{copyId}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            if (copy.OwnerId != user.Id)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "You are not the owner of this book.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            DeleteCopyVM delete = new DeleteCopyVM { CopyId = copyId, CurrentController = currentController };

            return await _json.RenderAsync("Copy/_Delete", delete);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _RemovePost(DeleteCopyVM obj)
        {
            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            Copy copy = _copies.GetById(obj.CopyId);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{obj.CopyId}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            if (copy.OwnerId != user.Id)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "You are not the owner of this book.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var result = _copies.Delete(obj.CopyId);

            if (result == -1)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "Something went wrong while Removing our book.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            message = new MessageBoxVM
            {
                Title = "",
                Message = "Your book was successfully removed."
            };

            if (obj.CurrentController == "copies")
            {
                return await _json.RenderAsync(new JsonVM { callback = "Copy/_IndexPartial" }, "_MessageBox", message);
            }
            else
            {
                return await _json.RenderAsync("_MessageBox", message);
            }
        }

        public async Task<IActionResult> _GetAvailableCopies(int editionId)
        {
            var user = await _userManager.GetUserAsync(User);

            string userId = null;

            if (user != null) userId = user.Id;

            return await _json.RenderAsync(new JsonVM { targetId = "#container-available-copies" },
                                                                                       "Copy/_AvailableCopies",
                                                                                       _copies.GetAvailableCopies(editionId, userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _WishCopyAdd(int copyId)
        {
            MessageBoxVM message;

            Copy copy = _copies.GetById(copyId);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{copyId}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            int matchedCopyId = _copies.FindMatch(copy.OwnerId, user.Id);


            if (_copies.WishCopieAdd(copyId, user.Id) == -1)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Something went wrong while registering your wish.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            string notif;

            if (matchedCopyId == -1)
            {
                notif = $"connection.invoke('SendNotification', '{copy.OwnerId}', '{user.Id}', {matchedCopyId}, {copyId}, '{user.Id}', 4);";
            }
            else
            {
                notif = $"connection.invoke('SendNotification', '{copy.OwnerId}', '{user.Id}', {matchedCopyId}, {copyId}, null, 0);";
            }
            return Ok(new { targetId = $"#{copyId}", responseText = "<div class='w-100 text-center text-success'><h6>Book Added to your wish List</h6></div>", notification = notif });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _WishMatch(int copyId, int initialCopyId, string ownerId) //Peut désormais être remplacé par WishCopyAdd
        {
            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            Copy copy = _copies.GetById(copyId);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{copyId}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            if (_copies.WishCopieAdd(copyId, user.Id) == -1)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Something went wrong while registering your wish.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            string notif = $"connection.invoke('SendNotification', '{ownerId}', '{user.Id}', {initialCopyId}, {copyId}, null, 0);";

            return Ok(new { targetId = $"#{copyId}", responseText = "<div class='w-100 text-center text-success'><h6>Book Added to your wish List</h6></div>", notification = notif });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _WishCopyRemove(int copyId)
        {
            var user = await _userManager.GetUserAsync(User);

            MessageBoxVM message;

            if (user == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Current user not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            Copy copy = _copies.GetById(copyId);

            if (copy == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Book Copy with id '{copyId}' not Found.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }
            if (_copies.WishCopieRemove(copyId, user.Id) == -1)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = $"Something went wrong while removing your wish.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            return _json.Render(new JsonVM { targetId = $"#{copyId}", responseText = "<div class='w-100 text-center text-success'><h6>Book removed from your wish List</h6></div>" });
        }
        private string OutputModelState(ModelStateDictionary modelState)
        {
            string errors = "";
            errors += string.Join("/n", modelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
            return errors;
        }
    }
}
