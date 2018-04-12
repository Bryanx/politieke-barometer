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
    
    private UserManager userManager = new UserManager();

    // GET
    public ActionResult Index()
    {
      const string ADMIN_DASHBOARD_PAGE_TITLE = "Admin Dashboard";
      return HttpNotFound();
    }

    public ActionResult PageManagement()
    {
      const string PAGE_MANAGEMENT_PAGE_TITLE = "Pagina's beheren";
      return View(new BaseViewModel() {
        User = userManager.GetUser(User.Identity.GetUserId()),
        PageTitle = PAGE_MANAGEMENT_PAGE_TITLE
      });
    }

    public ActionResult ItemManagement()
    {
      const string ITEM_MANAGEMENT_PAGE_TITLE = "Items beheren";
      IItemManager itemManager = new ItemManager();
      return View(new ItemViewModels.ItemViewModel() {
        User = userManager.GetUser(User.Identity.GetUserId()),
        PageTitle = ITEM_MANAGEMENT_PAGE_TITLE,
        Items = Mapper.Map(itemManager.GetAllItems(), new List<ItemDTO>())
      });
    }

    public ActionResult UserManagement()
    {
      const string USER_MANAGEMENT_PAGE_TITLE = "Gebruikers beheren";
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
        Users = users
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