using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UI.MVC.ActionFilters {

    public class CopyCultureCookieToRequestHeaderFilter : IActionFilter {

        public void OnActionExecuting (ActionExecutingContext context) {
            bool cultureCookieIsSet = context.HttpContext.Request.Cookies.TryGetValue (CookieRequestCultureProvider.DefaultCookieName, out string cultureCookie);
            if (cultureCookieIsSet) {
                context.HttpContext.Request.Headers.Add ("X-Culture", cultureCookie);
            }
        }

        public void OnActionExecuted (ActionExecutedContext context) {
            // Not implemented
        }

    }

}