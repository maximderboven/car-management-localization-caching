using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using UI.MVC.Controllers;

#nullable enable
namespace UI.MVC.ActionFilters {

    public class SetUserLanguageAsDefaultFilters : IActionFilter {

        private readonly IOptions<RequestLocalizationOptions> _options;
        private bool _hasChanged;

        public SetUserLanguageAsDefaultFilters (IOptions<RequestLocalizationOptions> options) {
            _options = options;
        }

        public void OnActionExecuting (ActionExecutingContext context) {

            if (context.HttpContext.Request.Cookies.TryGetValue (CookieRequestCultureProvider.DefaultCookieName, out _))
                return; // Default culture is already provided

            _hasChanged = true;
            IEnumerable<string> acceptedCultures = context.HttpContext.Request.GetTypedHeaders ()
                .AcceptLanguage.OrderByDescending (x => x.Quality ?? 1)
                .Select (requestHeader => requestHeader.Value.ToString ());

            IEnumerable<CultureInfo> supportedCultures = _options.Value.SupportedUICultures;
            foreach (var acceptedCulture in acceptedCultures) {

                CultureInfo? matchedCulture = supportedCultures.SingleOrDefault (c => c.Name == acceptedCulture);
                if (matchedCulture is not null) {
                    SetCulture (context, matchedCulture);
                    return; // Found culture
                }

                CultureInfo? matchedParentCulture = supportedCultures.FirstOrDefault (c => c.Parent.Name == acceptedCulture);
                if (matchedParentCulture is not null) {
                    SetCulture (context, matchedParentCulture);
                    return; // Found culture
                }
            }

            // Found no cultures and returned to default culture
            SetCulture (context, _options.Value.DefaultRequestCulture.Culture);

        }

        private static void SetCulture (ActionContext context, CultureInfo cultureInfo) {
            string culture = cultureInfo.ToString ();
            string cookie = CookieRequestCultureProvider.MakeCookieValue (new RequestCulture (culture));
            string cookieKey = CookieRequestCultureProvider.DefaultCookieName;
            context.HttpContext.Response.Cookies.Append (cookieKey, cookie);
        }

        public void OnActionExecuted (ActionExecutedContext context) {
            if (!_hasChanged)
                return;
            context.Result = new RedirectToRouteResult (context.ActionDescriptor.RouteValues);
        }

    }

}