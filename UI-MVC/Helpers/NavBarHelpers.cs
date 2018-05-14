using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Helpers {
    public static class NavBarHelpers {
        public static MvcHtmlString ShowAdminLinks(this HtmlHelper helper, IPrincipal user) {
            if (user.IsInRole("Admin") || user.IsInRole("SuperAdmin")) return helper.Partial("Partials/_NavBarAdminLinks");
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString ShowSuperAdminLinks(this HtmlHelper helper, IPrincipal user) {
            if (user.IsInRole("SuperAdmin")) return helper.Partial("Partials/_NavBarSuperAdminLinks");
            return MvcHtmlString.Empty;
        }
        
        public static MvcHtmlString ShowNavLogin(this HtmlHelper helper, BaseViewModel model) {
            if (model.User != null) return helper.Partial("Partials/_LoggedInNavbar", model);
            return helper.Partial("Partials/_LoginNavbar", new LoginViewModel());
        }

        public static MvcHtmlString FooterHelper(this HtmlHelper helper, BaseViewModel model) {
            if (model.User == null) return helper.Partial("Partials/_FooterRegister", new RegisterViewModel());
            return MvcHtmlString.Empty;
        }
        
        public static MvcHtmlString LeftNavBarHelper(this HtmlHelper helper, BaseViewModel model) {
            if (model.User == null) return helper.Partial("Partials/_LoggedInLeftNavBarLinks");
            return helper.Partial("Partials/_VisitorLeftNavBarLinks");
        }
    }
}