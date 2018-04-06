using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BAR.UI.MVC.Controllers {
    public class AdminController : Controller {
        // GET
        public ActionResult Index() {
            const string ADMIN_DASHBOARD_PAGE_TITLE = "Admin Dashboard";
            BaseViewModel vm = GetVm(ADMIN_DASHBOARD_PAGE_TITLE);
            return View(vm);

        }
        
        public ActionResult PageManagement() {
            const string PAGE_MANAGEMENT_PAGE_TITLE = "Pagina's beheren";
            return View(GetVm(PAGE_MANAGEMENT_PAGE_TITLE));

        }
        
        public ActionResult ItemManagement() {
            const string ITEM_MANAGEMENT_PAGE_TITLE = "Items beheren";
            return View(GetVm(ITEM_MANAGEMENT_PAGE_TITLE));

        }
        
        public ActionResult UserManagement() {
            const string USER_MANAGEMENT_PAGE_TITLE = "Gebruikers beheren";
            BaseViewModel vm = GetVm(USER_MANAGEMENT_PAGE_TITLE);
            UserManager userManager = new UserManager();
            return View(new EditUserViewModel(vm, userManager.GetAllUsers()));
        }
        

        /// <summary>
        /// Checks if the user has any special roles.
        /// If it has, the given model is updated.
        /// </summary>
        private BaseViewModel GetVm(string pageTitle) {
            IUserManager userManager = new UserManager();
            User user = userManager.GetUser(User.Identity.GetUserId());
            IList<string> userRoles = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>().GetRoles(User.Identity.GetUserId());
            return new BaseViewModel() {
                User = user,
                PageTitle = pageTitle,
                IsAdmin = userRoles.Contains("Admin"),
                IsSuperAdmin = userRoles.Contains("SuperAdmin")
            };
        }
    }
}