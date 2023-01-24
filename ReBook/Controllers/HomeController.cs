namespace ReBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEditionService _editions;
        private readonly IWishService _wishes;
        private readonly IEditionAPIService _google;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _mail;

        public HomeController(
                                IEditionService editions,
                                IWishService wishes,
                                UserManager<ApplicationUser> userManager,
                                IEditionAPIService google,
                                IJsonRenderService viewRenderService,
                                IEmailService mail)
        {
            _userManager = userManager;
            _mail = mail;
            _google = google;
            _editions = editions;
            _wishes = wishes;
        }
        public IActionResult Index(string userId, string email, string token, string returnUrl, string remoteError, bool externalLogin)
        {
            if (externalLogin)
            {
                TempData["msg"] = "<script>$(document).ready(function () {externalLogin('" + returnUrl + "','" + remoteError + "');});</script>";
            }
            else if (email is not null && token is not null)
            {
                TempData["msg"] = "<script>$(document).ready(function () {resetPassword('" + email + "','" + token + "');});</script>";
            }
            else if (userId is not null && token is not null)
            {
                TempData["msg"] = "<script>$(document).ready(function () {confirmEmail('" + userId + "','" + token + "');});</script>";
            }
            else if (returnUrl is not null)
            {
                TempData["msg"] = "<script>$(document).ready(function () {loginFilter('" + returnUrl + "');});</script>";
            }

            EditionIndexVM index = new();

            index.Query.Repo = "google";
            index.Query.CurrentController = "editions";

            return View(index);
        }

        [HttpGet]
        public async Task<IActionResult> _IndexPartial(QueryVM query)
        {
            Object result = null;

            if (ModelState.IsValid)
            {
                switch (query.Repo)
                {
                    case "local":
                        result = _editions.Get(query);
                        break;

                    case "google":
                        result = await _google.GetAdvanced(query);
                        break;

                    case "wishes":
                        if (query.CurrentController == "wishes")
                        {
                            var user = await _userManager.GetUserAsync(User);

                            if (user != null) query.UserId = user.Id;

                            result = _wishes.Get(query);
                        }
                        else
                        {
                            result = _editions.GetWished(query);
                        }
                        break;

                    case "copies":
                        if (query.CurrentController != "copies")
                        {
                            result = _editions.GetAvailables(query);
                        }
                        break;
                }
            }

            return PartialView("_IndexPartial", result);
        }

        [HttpGet]
        public async Task<IActionResult> _ClearSearch(string repo, string control)
        {
            var user = await _userManager.GetUserAsync(User);

            string userId = null;

            if (user != null) userId = user.Id;

            QueryVM query = new() { Repo = repo, CurrentController = control, UserId = userId };

            return RedirectToAction("_IndexPartial", "Home", query);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> _Contact()
        {
            MailRequestVM mailRequest = new();

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                mailRequest.FromEmail = user.Email;
            }

            return PartialView("_Contact", mailRequest);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateReCaptcha]
        public async Task<IActionResult> _Contact(MailRequestVM mail)
        {
            var message = new MessageBoxVM
            {
                Title = "Contact",
                Message = "Thanks for your message."
            };

            try
            {
                await _mail.SendEmailAsync(mail);

                return PartialView("_MessageBox", message);
            }
            catch
            {
                message.Error = true;
                message.Message = "Something went wrong while sending your message. Please try again later.";
                return PartialView("_MessageBox", message);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
