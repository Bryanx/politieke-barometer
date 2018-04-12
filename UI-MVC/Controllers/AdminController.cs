using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BAR.UI.MVC.Controllers
{
  [Authorize(Roles ="Admin, SuperAdmin")]
  public class AdminController : Controller
  {
    /// <summary>
    /// Dashboard page of admin.
    /// </summary>
    public ActionResult Index()
    {
      const string ADMIN_DASHBOARD_PAGE_TITLE = "Admin Dashboard";
      return HttpNotFound();
    }

    /// <summary>
    /// Page management page of admin.
    /// </summary>
    public ActionResult PageManagement()
    {
      const string PAGE_MANAGEMENT_PAGE_TITLE = "Pagina's beheren";
      UserManager userManager = new UserManager();
      return View(new BaseViewModel() {
        User = userManager.GetUser(User.Identity.GetUserId()),
        PageTitle = PAGE_MANAGEMENT_PAGE_TITLE
      });
    }

    /// <summary>
    /// Item management page of admin.
    /// </summary>
    public ActionResult ItemManagement()
    {
      const string ITEM_MANAGEMENT_PAGE_TITLE = "Items beheren";
      UserManager userManager = new UserManager();
      IItemManager itemManager = new ItemManager();
      return View(new ItemViewModels.ItemViewModel() {
        User = userManager.GetUser(User.Identity.GetUserId()),
        PageTitle = ITEM_MANAGEMENT_PAGE_TITLE,
        Items = Mapper.Map(itemManager.GetAllItems(), new List<ItemDTO>())
      });
    }

    /// <summary>
    /// User management page of admin.
    /// </summary>
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
        User = userManager.GetUser(User.Identity.GetUserId()),
        PageTitle = USER_MANAGEMENT_PAGE_TITLE,
        Users = users,
        Roles = roles
      };
      return View(vm);
    }
  }
}