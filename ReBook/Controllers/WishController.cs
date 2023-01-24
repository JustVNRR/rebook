namespace ReBook.Controllers
{
    [LoginFilter]
    public class WishController : Controller
    {
        private readonly IJsonRenderService _json;
        private readonly IWishService _wishes;
        private readonly UserManager<ApplicationUser> userManager;

        public WishController(
                                    IWishService wishes,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IJsonRenderService viewRenderService)
        {
            this.userManager = userManager;
            _wishes = wishes;
            _json = viewRenderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            return View(_wishes.Get(new QueryVM { UserId = user.Id, CurrentController = "wishes", Repo = "wishes" }));
        }

        //[HttpPost]
        public async Task<IActionResult> _Add(string userId, int editionId, string currentController)
        {
            Wish result = _wishes.Add(new Wish { UserId = userId, EditionId = editionId });

            if (result == null)
            {
                var message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "Something went wrong while registering your wish.",
                    Error = true
                };

                return await _json.RenderAsync("_MessageBox", message);
            }
            else
            {
                //Chercher si des copies sont disponibles à l'échange
                // Si tel est le cas afficher leur liste
                //Sinon 

                var message = new MessageBoxVM
                {
                    Title = "",
                    Message = "Your wish was successfully registered.",
                };

                if (currentController == "editions")
                {
                    return await _json.RenderAsync("_MessageBox", message);
                }
                else
                {
                    return await _json.RenderAsync(new JsonVM { callback = "Home/_IndexPartial" }, "_MessageBox", message);
                }
            }
        }

        //[HttpPost]
        public async Task<IActionResult> _Delete(string userId, int editionId, string currentController)
        {
            var result = _wishes.Delete(new Wish { UserId = userId, EditionId = editionId });

            if (result == -1)
            {
                var message = new MessageBoxVM
                {
                    Title = "Unexpected error",
                    Message = "Something went wrong while cancelling your wish.",
                    Error = true
                };

                return await _json.RenderAsync("_MessageBox", message);
            }
            else
            {
                var message = new MessageBoxVM
                {
                    Title = "",
                    Message = "Your're not looking for this book anymore"
                };

                if (currentController == "editions")
                {
                    return await _json.RenderAsync("_MessageBox", message);
                }
                else
                {
                    return await _json.RenderAsync(new JsonVM { callback = "$('#main-search-button').trigger('click');" }, "_MessageBox", message);
                }
            }
        }
    }
}
