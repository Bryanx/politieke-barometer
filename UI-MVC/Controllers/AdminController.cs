using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BAR.UI.MVC.Controllers
{
  public class AdminController : Controller
  {
    // GET
    public ActionResult Index()
    {
      const string ADMIN_DASHBOARD_PAGE_TITLE = "Admin Dashboard";
      return HttpNotFound();
    }

    public ActionResult PageManagement()
    {
      const string PAGE_MANAGEMENT_PAGE_TITLE = "Pagina's beheren";
      return HttpNotFound();
    }

    public ActionResult ItemManagement()
    {
      const string ITEM_MANAGEMENT_PAGE_TITLE = "Items beheren";
      return HttpNotFound();
    }

    public ActionResult UserManagement()
    {
      const string USER_MANAGEMENT_PAGE_TITLE = "Gebruikers beheren";
      UserManager userManager = new UserManager();
      IdentityUserManager identityUserManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
      IEnumerable<User> users = userManager.GetAllUsers();
      List<string> currentRoles = new List<string>();
      for (int i = 0; i < users.Count(); i++)
      {
        currentRoles.Add(identityUserManager.GetRoles(users.ElementAt(i).Id).FirstOrDefault());
      }
      ViewBag.CurrentRoles = currentRoles;
      var roles = userManager.GetAllRoles().Select(x => new SelectListItem
      {
        Value = x.Id,
        Text = x.Name,
      }).OrderBy(x => x.Text);
      EditUserViewModel vm = new EditUserViewModel()
      {
        PageTitle = USER_MANAGEMENT_PAGE_TITLE,
        Users = userManager.GetAllUsers(),
        Roles = roles
      };
      return View(vm);
    }
  }
}