namespace ReBook.Controllers
{
    public class EditionController : Controller
    {
        private readonly IEditionService _editions;
        private readonly IEditionAPIService _google;
        private readonly IWishService _wishes;
        private readonly IJsonRenderService _json;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IDataProtector _protector;

        public EditionController(
                                UserManager<ApplicationUser> userManager,
                                IEditionService editions,
                                IWishService wishes,
                                SignInManager<ApplicationUser> signInManager,
                                IEditionAPIService google,
                                IJsonRenderService viewRenderService,
                                Cloudinary cloudinary/*,
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings*/)
        {
            _google = google;
            _editions = editions;
            _wishes = wishes;
            _userManager = userManager;
            _signInManager = signInManager;
            _json = viewRenderService;
            //_protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.UserIdRouteValue);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_editions.Get(new QueryVM { Repo = "local", CurrentController = "editions" }));
        }

        public async Task<IActionResult> _AddAuthor(EditionVM obj)
        {
            obj.Edition.Book.Authors.Add(new Author());
            return await _json.RenderAsync(new JsonVM { targetId = "#authorsListContainer" }, "Edition/_AddAuthorPartial", obj);
        }

        public async Task<IActionResult> _UpdateAuthors(EditionVM obj)
        {
            return await _json.RenderAsync(new JsonVM { targetId = "#authorsListContainer" }, "Edition/_AddAuthorPartial", obj);
        }

        public async Task<IActionResult> _AddTag(EditionVM obj)
        {
            obj.Edition.Book.Tags.Add(new Tag());
            return await _json.RenderAsync(new JsonVM { targetId = "#tagsListContainer" }, "Edition/_AddTagPartial", obj);
        }

        public async Task<IActionResult> _UpdateTags(EditionVM obj)
        {
            return await _json.RenderAsync(new JsonVM { targetId = "#tagsListContainer" }, "Edition/_AddTagPartial", obj);
        }

        [LoginFilter]
        public async Task<IActionResult> _ISBN(string currentController)
        {
            return await _json.RenderAsync("Edition/_ISBN", new ISBNVM { CurrentController = currentController });
        }

        public async Task<IActionResult> _FindByContextLink(string name, string repo, string context)
        {
            QueryVM query = new()
            {
                SearchContext = context,
                Repo = repo
            };
            if (context == "author") query.Author = name;
            else if (context == "title") query.Title = name;
            else if (context == "editor") query.Editor = name;
            else if (context == "tag") query.Tag = name;

            switch (query.Repo)
            {
                case "local":
                    return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Home/_IndexPartial", _editions.Get(query));
                case "google":
                    return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Home/_IndexPartial", await _google.GetAdvanced(query));
                case "wishes":
                    if (query.CurrentController == "wishes")
                    {
                        var user = await _userManager.GetUserAsync(User);
                        if (user != null) query.UserId = user.Id;
                        return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Home/_IndexPartial", _wishes.Get(query));
                    }
                    else
                    {
                        return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Home/_IndexPartial", _editions.GetWished(query));
                    }
                case "copies":
                    if (query.CurrentController == "copies")
                    {
                        return null;
                        /*var user = await userManager.GetUserAsync(User);
                        if (user != null) query.UserId = user.Id;
                        return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Home/_IndexPartial", _copies.Get(query));*/
                    }
                    else
                    {
                        return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" }, "Home/_IndexPartial", _editions.GetAvailables(query));
                    }
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
        public async Task<IActionResult> _FIND_ISBN(ISBNVM isbnVM)
        {
            EditionVM editionViewModel = new () { Repository = isbnVM.Repository, CurrentController = isbnVM.CurrentController };

            Edition edition = _editions.GetByISBN(isbnVM.ISBN);

            if (edition is null) edition = await _google.GetByISBN(isbnVM.ISBN);

            if (edition is not null)
            {
                editionViewModel.Edition = edition;

                editionViewModel.PageTitle = edition.Book.Title;

                if (editionViewModel.PageTitle.Length > 35) editionViewModel.PageTitle = (editionViewModel.PageTitle).Substring(0, 35) + "...";

                if (edition.Id == 0 && _signInManager.IsSignedIn(User) && User.IsInRole("admin"))
                {
                    // return await _json.RenderAsync("Edition/_CreateOrEdit", editionViewModel);
                    return PartialView("_CreateOrEdit", editionViewModel);
                }
                else
                {
                    if (_signInManager.IsSignedIn(User))
                    {
                        var user = await _userManager.GetUserAsync(User);
                        editionViewModel.IsUserLookingFor = _editions.IsUserLookingFor(edition.Id, user.Id);

                        if (editionViewModel.IsUserLookingFor is true)
                        {
                            //
                        }
                        editionViewModel.IsUserOwnerOf = _editions.IsUserOwnerOf(edition.Id, user.Id);

                        if (editionViewModel.IsUserOwnerOf is false)
                        {
                            editionViewModel.AvailableCopies = _editions.GetAvailableCopiesCount(edition.Id);
                        }
                    }

                    // return await _json.RenderAsync("Edition/_Details", editionViewModel);
                    return PartialView("_Details", editionViewModel);
                }
            }
            else
            {
                if (_signInManager.IsSignedIn(User) && User.IsInRole("admin"))
                {
                    editionViewModel.PageTitle = "New Edition";
                    editionViewModel.Edition.ISBN13 = isbnVM.ISBN;
                    // return await _json.RenderAsync("Edition/_CreateOrEdit", editionViewModel);
                    return PartialView("_CreateOrEdit", editionViewModel);

                }
                else
                {
                    var message = new MessageBoxVM
                    {
                        Title = "Edit Edition",
                        Message = "Edition not found",
                        Error = true
                    };

                    // return await _json.RenderAsync("_MessageBox", message);
                    return PartialView("_MessageBox", message);

                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateOrEditPost(EditionVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Edition/_CreateOrEdit", obj);
            }

            obj.PageTitle = "Edition Details";

            if (obj.Edition.Id == 0)
            {
                obj.Edition = _editions.Create(obj.Edition);
            }
            else
            {
                obj.Edition = _editions.Update(obj.Edition);
            }

            /*if (obj.Repository == "local")
            {
                // return await _json.RenderAsync(new JsonVM { callback = "$('#main-search-button').trigger('click');" }, "Edition/_Details", obj);
                return PartialView("_Details", obj);
            }
            else
            {
                // return await _json.RenderAsync("Edition/_Details", obj);
                return PartialView("_Details", obj);
            }*/
            return PartialView("_Details", obj);
        }

        [HttpPost]
        public string SaveCover(IFormCollection fileName, IFormFile file)
        {
            return _editions.SaveCover(fileName["name"], file);
        }

        [HttpGet]
        public async Task<IActionResult> _EditGet(int Id)
        {
            Edition edition = _editions.GetById(Id);

            if (edition != null)
            {
                var editionViewModel = new EditionVM
                {
                    PageTitle = "Edit Edition",
                    Edition = edition
                };
                return await _json.RenderAsync("Edition/_CreateOrEdit", editionViewModel);

            }
            var message = new MessageBoxVM
            {
                Title = "Edit Edition",
                Message = "Edition not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpGet]
        public async Task<IActionResult> _DeleteGet(int id)
        {
            Edition edition = _editions.GetById(id);

            if (edition != null)
            {
                var editionViewModel = new DeleteEditionVM
                {
                    PageTitle = "Delete Edition",
                    Id = edition.Id,
                    Book = edition.Book.Title
                };
                editionViewModel.ISBN = edition.ISBN13;
                editionViewModel.Editor = edition.Editor;
                editionViewModel.Cover = edition.Cover;

                return await _json.RenderAsync("Edition/_Delete", editionViewModel);
            }
            var message = new MessageBoxVM
            {
                Title = "Delete Edition",
                Message = "Edition not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeletePost(DeleteEditionVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                // return await _json.RenderAsync("Edition/_CreateOrEdit", obj);
                return PartialView("_CreateOrEdit", obj);
            }

            var message = new MessageBoxVM
            {
                Title = "Delete Edition"
            };
            if (_editions.Delete(obj.Id) == 1)
            {
                message.Message = "Edition successfully deleted";
                // return await _json.RenderAsync(new JsonVM { callback = "$('#main-search-button').trigger('click');" }, "_MessageBox", message);
                // return PartialView("_MessageBox", message);
            }
            else if (_editions.Delete(obj.Id) == -1)
            {
                message.Message = "Edition not Found";
                message.Error = true;
                // return await _json.RenderAsync("_MessageBox", message);
                // return PartialView("_MessageBox", message);
            }
            else
            {
                message.Message = "An error occured while deleting the Edition";
                message.Error = true;
                //return await _json.RenderAsync("_MessageBox", message);
            }

            return PartialView("_MessageBox", message);
        }

        [HttpGet]
        [LoginFilter]
        //[ServiceFilter(typeof(AuthorisationFilter))]
        public IActionResult _Details(/*int id, */string isbn, string repo, string currentController)
        {
            return RedirectToAction("_FIND_ISBN", "Edition", new ISBNVM { ISBN = isbn, Repository = repo, CurrentController = currentController });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _WishAdd(EditionVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Edition/_Details", obj);
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

            Edition edition = _editions.GetById(obj.Edition.Id);

            if (edition == null)
            {
                edition = await _google.GetByISBN(obj.Edition.ISBN13);

                if (edition == null)
                {
                    var message = new MessageBoxVM
                    {
                        Title = "Error while getting current book ",
                        Message = "Book not found",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }
                else
                {
                    edition = _editions.Create(edition);

                    if (edition == null)
                    {
                        var message = new MessageBoxVM
                        {
                            Title = "Unexpected error",
                            Message = "An error occurend while adding current book to our local database.",
                            Error = true
                        };

                        return await _json.RenderAsync("_MessageBox", message);
                    }
                }
            }

            return RedirectToAction("_Add", "Wish", new { userId = user.Id, editionId = edition.Id, currentController = obj.CurrentController });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _WishDelete(EditionVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Edition/_Details", obj);
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

            Edition edition = _editions.GetById(obj.Edition.Id);

            if (edition == null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "Edition not found.",
                    Error = true
                };

                return await _json.RenderAsync("_MessageBox", message);
            }

            return RedirectToAction("_Delete", "Wish", new { userId = user.Id, editionId = edition.Id, currentController = obj.CurrentController });
        }

        [LoginFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddCopyGet(EditionVM obj)
        {
            if (!ModelState.IsValid)
            {
                obj.Error = OutputModelState(ModelState);

                return await _json.RenderAsync("Edition/_Details", obj);
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

            Edition edition = _editions.GetById(obj.Edition.Id);

            if (edition == null)
            {
                edition = await _google.GetByISBN(obj.Edition.ISBN13);

                if (edition == null)
                {
                    var message = new MessageBoxVM
                    {
                        Title = "Error while getting current book ",
                        Message = "Book not found",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }
                else
                {
                    edition = _editions.Create(edition);

                    if (edition == null)
                    {
                        var message = new MessageBoxVM
                        {
                            Title = "Unexpected error",
                            Message = "An error occurend while adding current book to our local database.",
                            Error = true
                        };

                        return await _json.RenderAsync("_MessageBox", message);
                    }
                }
            }

            return RedirectToAction("_AddGet", "Copy", new { userId = user.Id, editionId = edition.Id, currentController = obj.CurrentController });
        }

        [HttpGet]
        public async Task<IActionResult> _Loader()
        {
            return await _json.RenderAsync("_Loader");
        }

        [HttpGet]
        [AllowAnonymous]
        public List<string> GetAuthors()
        {
            return _editions.GetAuthorsList(); ;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<string> GetBooks()
        {
            return _editions.GetBooksList(); ;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<string> GetEditors()
        {
            return _editions.GetEditorsList(); ;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<string> GetLanguages()
        {
            return Enum.GetNames(typeof(Language)).ToList();
        }

        [HttpGet]
        [AllowAnonymous]
        public List<string> GetTags()
        {
            return _editions.GetTagsList();
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
