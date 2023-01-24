namespace ReBook.Controllers
{
    [LoginFilter]
    public class NotificationController : Controller
    {
        private readonly IJsonRenderService _json;
        private readonly INotificationService _notifs;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITradeService _trades;

        public NotificationController(
                                    INotificationService notifs,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IJsonRenderService viewRenderService,
                                    ITradeService trades)
        {
            _userManager = userManager;
            _notifs = notifs;
            _json = viewRenderService;
            _trades = trades;
        }

        public async Task<ActionResult> _GetNotifications()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var notifications = _notifs.GetUserNotifications(userId, true);

            NotificationIndexVM notifsVM = new();

            if (notifications != null && notifications.Count > 0)
            {
                notifsVM.Notifications = notifications;
            }

            return await _json.RenderAsync("Notification/_Index", notifsVM);
        }

        public async Task<ActionResult> CheckNotifications()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Ok(new { count = 0 });
            else
            {
                int count = _notifs.GetUnreadNumber(user.Id);
                return Ok(new { count });
            }
        }

        public IActionResult _ReadNotification(int notificationId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            _notifs.ReadNotification(notificationId, userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> _AcceptMatch(int notificationId)
        {
            MessageBoxVM message;

            Notification notif = _notifs.GetById(notificationId);

            if (notif == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Notification not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            if (notif.Status != NotiStatus.unread)
            {
                message = new MessageBoxVM
                {
                    Title = "To late",
                    Message = "Sorry this notification is out of date.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            notif.Status = NotiStatus.accepted;

            if (_notifs.Update(notif) != 1)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected Error",
                    Message = "Something went wrong while registering your choice.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            if (notif.Type == NotiType.match)
            {
                string strNotif = $"connection.invoke('SendNotification', '{notif.ToUserId}', '{notif.FromUserId}', {notif.FromCopyId}, {notif.ToCopyId}, '{userId}', 1);";
                return await _json.RenderAsync(new JsonVM { targetId = $"#{notificationId}", notification = strNotif }, "Notification/_Accepted", notif);
            }
            else if (notif.Type == NotiType.accepted)
            {
                Trade trade = new Trade
                {
                    FromUserId = notif.FromUserId,
                    FromUserName = notif.FromUserName,
                    FromUserLogin = notif.FromUserLogin,
                    FromUserAddress = notif.FromUserAddress,
                    FromCopyId = notif.FromCopyId,
                    FromCopyVisual = notif.FromCopyVisual,
                    FromCopyTitle = notif.FromCopyTitle,

                    ToUserId = notif.ToUserId,
                    ToUserName = notif.ToUserName,
                    ToUserLogin = notif.ToUserLogin,
                    ToUserAddress = notif.ToUserAddress,
                    ToCopyId = notif.ToCopyId,
                    ToCopyVisual = notif.ToCopyVisual,
                    ToCopyTitle = notif.ToCopyTitle
                };

                if (_trades.Add(trade) == 3)
                {
                    string strNotif = $"connection.invoke('SendNotification', '{notif.ToUserId}', '{notif.FromUserId}', {notif.FromCopyId}, {notif.ToCopyId}, '{userId}', 2);";
                    return await _json.RenderAsync(new JsonVM { targetId = $"#{notificationId}", notification = strNotif }, "Notification/_NewTradeNotification", notif);
                }
                else if (_trades.Add(trade) == -10)
                {
                    message = new MessageBoxVM
                    {
                        Title = "Unexpected Error",
                        Message = $"Copy of {notif.FromCopyTitle} not found.",
                        Error = true
                    };

                    return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                }
                else if (_trades.Add(trade) == -11)
                {
                    message = new MessageBoxVM
                    {
                        Title = "To late...",
                        Message = $"Copy of {notif.FromCopyTitle} is no more available.",
                        Error = true
                    };

                    return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                }
                else if (_trades.Add(trade) == -20)
                {
                    message = new MessageBoxVM
                    {
                        Title = "Unexpected Error",
                        Message = $"Copy of {notif.ToCopyTitle} not found.",
                        Error = true
                    };

                    return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                }
                else if (_trades.Add(trade) == -21)
                {
                    message = new MessageBoxVM
                    {
                        Title = "To late...",
                        Message = $"Copy of {notif.ToCopyTitle} is no more available.",
                        Error = true
                    };

                    return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                }
                else
                {
                    message = new MessageBoxVM
                    {
                        Title = "Unexpected Error",
                        Message = "Something went wrong while registering your choice.",
                        Error = true
                    };

                    return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
                }
            }

            message = new MessageBoxVM
            {
                Title = "Unexpected Error",
                Message = "Something went wrong while registering your choice.",
                Error = true
            };

            return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);

        }

        [HttpPost]
        public async Task<IActionResult> _RefuseMatch(int notificationId)
        {
            MessageBoxVM message;

            Notification notif = _notifs.GetById(notificationId);

            if (notif == null)
            {
                message = new MessageBoxVM
                {
                    Title = "Error",
                    Message = "Notification not found",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            if (notif.Status != NotiStatus.unread)
            {
                message = new MessageBoxVM
                {
                    Title = "To late",
                    Message = "Sorry this notification is out of date.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            notif.Status = NotiStatus.refused;

            if (_notifs.Update(notif) != 1)
            {
                message = new MessageBoxVM
                {
                    Title = "Unexpected Error",
                    Message = "Something went wrong while registering your choice.",
                    Error = true
                };

                return await _json.RenderAsync(new JsonVM { success = false }, "_MessageBox", message);
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            string strNotif = $"connection.invoke('SendNotification', '{notif.ToUserId}', '{notif.FromUserId}', {notif.FromCopyId}, {notif.ToCopyId}, '{userId}', 5);";
            return await _json.RenderAsync(new JsonVM { targetId = $"#{notificationId}", notification = strNotif }, "Notification/_Refused", notif);
        }

        /*[HttpPost]
        public async Task<IActionResult> _SendAddress(Notification notification)
        {
            Notification sendNotification = new Notification();

            if (_notifs.Create(sendNotification) == 1)
            {
                string notif = $"connection.invoke('SendNotification', '{ownerId}', '{user.Id}', {initialCopyId}, {copyId}, null, 0);";

                return Ok(new { targetId = $"#{copyId}", responseText = "<div class='w-100 text-center text-success'><h6>Book Added to your wish List</h6></div>", notification = notif });

            }
            return null;

        }*/
    }
}
