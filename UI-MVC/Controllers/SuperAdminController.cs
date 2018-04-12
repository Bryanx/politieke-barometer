using System.Web.Mvc;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;

namespace BAR.UI.MVC.Controllers
{

  [Authorize(Roles = "SuperAdmin")]
  public class SuperAdminController : Controller
  {
    /// <summary>
    /// Sourcemanagement page of the SuperAdmin.
    /// </summary>
    public ActionResult SourceManagement()
    {
      const string PAGE_TITLE = "Bronnen beheren";
      IUserManager userManager = new UserManager();
      return View(new BaseViewModel
      {
        PageTitle = PAGE_TITLE,
        User = userManager.GetUser(User.Identity.GetUserId())
      });
    }

    /// <summary>
    /// Adminmanagement page of the SuperAdmin.
    /// </summary>
    public ActionResult AdminManagement()
    {
      const string PAGE_TITLE = "Admins beheren";
      IUserManager userManager = new UserManager();
      return View(new BaseViewModel
      {
        PageTitle = PAGE_TITLE,
        User = userManager.GetUser(User.Identity.GetUserId())
      });
    }
    /// <summary>
    /// Platformmanagement page of the SuperAdmin.
    /// </summary>
    public ActionResult PlatformManagement()
    {
      const string PAGE_TITLE = "Deelplatformen beheren";
      IUserManager userManager = new UserManager();
      return View(new BaseViewModel
      {
        PageTitle = PAGE_TITLE,
        User = userManager.GetUser(User.Identity.GetUserId())
      });
    }
  }
}