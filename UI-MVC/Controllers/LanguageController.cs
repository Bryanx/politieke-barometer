using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BAR.UI.MVC.Helpers;
using BAR.UI.MVC.Attributes;

namespace BAR.UI.MVC.Controllers {
    /// <summary>
    /// This controller stores the user preferred language in a cookie.
    /// </summary>
    [SubPlatformCheck]
    public class LanguageController : Controller {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state) {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : // obtain it from HTTP header AcceptLanguages
                    null;

            // Validate culture name
            cultureName = LanguageHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
}