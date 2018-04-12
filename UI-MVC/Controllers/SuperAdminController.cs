using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BAR.UI.MVC.Controllers {
    
    [Authorize(Roles="SuperAdmin")]
    public class SuperAdminController : Controller {
        
        IUserManager userManager = new UserManager();
        
        public ActionResult SourceManagement() {
            const string PAGE_TITLE = "Bronnen beheren";
            return View(new BaseViewModel {
                PageTitle = PAGE_TITLE,
                User = userManager.GetUser(User.Identity.GetUserId())
            });
        }       
        
        public ActionResult PlatformManagement() {
            const string PAGE_TITLE = "Deelplatformen beheren";
            return View(new BaseViewModel {
                PageTitle = PAGE_TITLE,
                User = userManager.GetUser(User.Identity.GetUserId())
            });
        }
    
        public ActionResult AdminManagement()
        {
            const string USER_MANAGEMENT_PAGE_TITLE = "Admins beheren";
            IdentityUserManager identityUserManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            IEnumerable<User> users = userManager.GetAllUsers();
            List<string> currentRoles = new List<string>();
            for (int i = 0; i < users.Count(); i++)
            {
                currentRoles.Add(identityUserManager.GetRoles(users.ElementAt(i).Id).FirstOrDefault());
            }
            ViewBag.CurrentRoles = currentRoles;
            EditUserViewModel vm = new EditUserViewModel()
            {
                User = userManager.GetUser(User.Identity.GetUserId()),
                PageTitle = USER_MANAGEMENT_PAGE_TITLE,
                Users = users,
            };
            FillViewModels(vm);
            return View(vm);
        }

        private void FillViewModels(EditUserViewModel vm) {
            vm.AdminRoles = userManager.GetAllRoles().Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name,
            }).OrderBy(x => x.Text);
            vm.UserRoles = userManager.GetAllRoles().Where(r=>r.Name == "Admin" || r.Name == "User")
                .Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name,
            }).OrderBy(x => x.Text);
        }
    }
}