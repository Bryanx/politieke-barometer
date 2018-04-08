using System.Web.Mvc;
using BAR.UI.MVC.Controllers;

namespace BAR.UI.MVC.Helpers {
    
    /// <summary>
    /// These helpers act as a table of contents of all URLs on the website.
    /// If a Controller/action is refactored it should be changed here.
    /// </summary>
    public static class UrlExtensions {
        public static string RootUrl(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Home",
                action = nameof(HomeController.Index)
            });
        }

        public static string NewsUrl(this UrlHelper helper) {
            return RootUrl(helper) + "#nieuws";
        }

        #region UserUrls
        
        public static string DashboardUrl(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "User",
                action = nameof(UserController.Index)
            });
        }

        public static string SettingsUrl(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "User",
                action = nameof(UserController.Settings)
            });
        }
        
        #endregion

        #region ItemUrls
        
        public static string PersonUrl(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Person",
                action = nameof(PersonController.Index)
            });
        }

        public static string PersonUrl(this UrlHelper helper, int personId) {
            return helper.RouteUrl("Default", new {
                controller = "Person",
                action = nameof(PersonController.Details),
                id = personId
            });
        }

        public static string OrganisationUrl(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Organisation",
                action = nameof(OrganisationController.Index)
            });
        }
        #endregion
        
        #region AdminUrls

        public static string AdminDashboard(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Admin",
                action = nameof(AdminController.Index)
            });
        }

        public static string PageManagement(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Admin",
                action = nameof(AdminController.PageManagement)
            });
        }

        public static string ItemManagement(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Admin",
                action = nameof(AdminController.ItemManagement)
            });
        }

        public static string UserManagement(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "Admin",
                action = nameof(AdminController.UserManagement)
            });
        }
        
        #endregion
        
        #region SuperAdminUrls

        public static string SourceManagement(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "SuperAdmin",
                action = nameof(SuperAdminController.SourceManagement)
            });
        }
        
        public static string AdminManagement(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "SuperAdmin",
                action = nameof(SuperAdminController.AdminManagement)
            });
        }
        
        public static string PlatformManagement(this UrlHelper helper) {
            return helper.RouteUrl("Default", new {
                controller = "SuperAdmin",
                action = nameof(SuperAdminController.PlatformManagement)
            });
        }

        #endregion
    }
}