namespace ReBook.Controllers
{
    [LoginFilter]
    public class TradeController : Controller
    {
        private readonly IJsonRenderService _json;
        private readonly ICopyService _copies;
        private readonly ITradeService _trades;
        private readonly UserManager<ApplicationUser> _userManager;

        public TradeController(
                                    ICopyService copies,
                                    ITradeService trades,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IJsonRenderService viewRenderService)
        {
            _userManager = userManager;
            _copies = copies;
            _trades = trades;
            _json = viewRenderService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(_trades.Get(new QueryTradeVM { UserId = user.Id }));
        }

        [HttpGet]
        public async Task<IActionResult> _IndexPartial(QueryTradeVM query)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                query.UserId = user.Id;
                query.CurrentController = "copies";


                return await _json.RenderAsync(new JsonVM { targetId = "#tbody-edition-index" },
                                                                                           "Trade/_IndexPartial",
                                                                                           _trades.Get(query));
            }

            var message = new MessageBoxVM
            {
                Title = "Invalid Form",
                Message = OutputModelState(ModelState),
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        private string OutputModelState(ModelStateDictionary modelState)
        {
            string errors = "";
            errors += string.Join("/n", modelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
            return errors;
        }

        [HttpGet]
        public async Task<IActionResult> _EditGet(int id)
        {
            Trade trade = _trades.GetById(id);

            if (trade != null)
            {
                var user = await _userManager.GetUserAsync(User);

                string book;
                if (user.Id == trade.ToUserId)
                {
                    book = trade.FromCopyTitle;
                }
                else
                {
                    book = trade.ToCopyTitle;

                }

                var tradeVM = new TradeVM
                {
                    TradeId = trade.Id,
                    UserId = user.Id,
                    Book = book
                };
                return await _json.RenderAsync("Trade/_Update", tradeVM);

            }
            var message = new MessageBoxVM
            {
                Title = "Trade Update",
                Message = $"Trade with id '{id}' not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }

        [HttpPost]
        public async Task<IActionResult> _UpdatePost(TradeVM tradeVM)
        {
            MessageBoxVM message;

            Trade trade = _trades.GetById(tradeVM.TradeId);

            if (trade != null)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user.Id == trade.FromUserId)
                {
                    if (tradeVM.Received == true)
                    {
                        trade.ToStatus = TradeStatus.complete;
                    }
                    else
                    {
                        trade.ToStatus = TradeStatus.pending;
                    }

                    trade.ToUserRating = tradeVM.Rating;
                    trade.ToUserComment = tradeVM.Comment;
                }
                else if (user.Id == trade.ToUserId)
                {
                    if (tradeVM.Received == true)
                    {
                        trade.FromStatus = TradeStatus.complete;
                    }
                    else
                    {
                        trade.FromStatus = TradeStatus.pending;
                    }

                    trade.FromUserRating = tradeVM.Rating;
                    trade.FromUserComment = tradeVM.Comment;
                }
                else
                {
                    message = new MessageBoxVM
                    {
                        Title = "Trade Update",
                        Message = $"Trade with id '{tradeVM.TradeId}' not found",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }

                if (trade.FromStatus == TradeStatus.complete && trade.ToStatus == TradeStatus.complete) trade.Status = TradeStatus.complete;

                Trade updatedTrade = _trades.Update(trade);

                if (updatedTrade.Equals(trade))
                {
                    //SendUpdateTradeNotification(int tradeId, string currentUserId)
                    string strNotif = $"connection.invoke('SendUpdateTradeNotification', {trade.Id}, '{user.Id}');";
                    message = new MessageBoxVM
                    {
                        Title = "Trade Update",
                        Message = $"Trade successfully updated."
                    };

                    return await _json.RenderAsync(new JsonVM { callback = "Trade/_IndexPartial", notification = strNotif }, "_MessageBox", message);
                }
                else
                {
                    message = new MessageBoxVM
                    {
                        Title = "Trade Update",
                        Message = $"Something went wrong while registering your update.",
                        Error = true
                    };

                    return await _json.RenderAsync("_MessageBox", message);
                }
            }

            message = new MessageBoxVM
            {
                Title = "Trade Update",
                Message = $"Trade with id '{tradeVM.TradeId}' not found",
                Error = true
            };

            return await _json.RenderAsync("_MessageBox", message);
        }
    }
}
