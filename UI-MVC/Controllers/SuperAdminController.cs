using System.Web.Mvc;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;

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
        
        public ActionResult AdminManagement() {
            const string PAGE_TITLE = "Admins beheren";
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
    }
}