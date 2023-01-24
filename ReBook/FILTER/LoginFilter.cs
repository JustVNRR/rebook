namespace ReBook.FILTER
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isLoggedIn = filterContext.HttpContext.Request.Cookies.ContainsKey(".AspNetCore.Identity.Application");

            if (!isLoggedIn)
            {
                var isAjax = filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                var path = filterContext.HttpContext.Request.Path;
                var query = filterContext.HttpContext.Request.QueryString;
                var pathAndQuery = path + query;

                if (isAjax)
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
                else
                {
                    //base.OnActionExecuting(filterContext);
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Home"},
                            {"action", "Index"},
                            { "ReturnUrl", pathAndQuery }
                        }
                    );
                }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
