namespace ReBook.FILTER
{
    public class AuthorisationFilter : ActionFilterAttribute
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthorisationFilter(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext,
                                         ActionExecutionDelegate next)
        {
            var isAjax = filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var user = await userManager.GetUserAsync(filterContext.HttpContext.User);
            var path = filterContext.HttpContext.Request.Path;
            var query = filterContext.HttpContext.Request.QueryString;
            var pathAndQuery = path + query;

            if (isAjax)
            {
                if (user == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Account"},
                            {"action", "_LoginRegisterGet"},
                            { "ReturnUrl", pathAndQuery }
                        }
                    );
                }
                else if (!await userManager.IsInRoleAsync(user, "admin"))
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                            {"controller", "Account"},
                            {"action", "_AccessDenied"}
                    });
                }
            }
            else
            {

                if (user == null || !await userManager.IsInRoleAsync(user, "admin"))
                {
                    filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                            {"controller", "Account"},
                            {"action", "AccessDenied"}
                 });
                }
            }

            await base.OnActionExecutionAsync(filterContext, next);
        }
    }
}
