using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
  public class HomeController : Controller
  {
    /// <summary>
    /// Landing page for logged-in and non-logged-in users.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    public ActionResult Index()
    {
      const string INDEX_PAGE_TITLE = "Politieke Barometer";
      IItemManager itemMgr = new ItemManager();
      UserManager userManager = new UserManager();
      return View(new ItemViewModel()
      {
        PageTitle = INDEX_PAGE_TITLE,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
        Items = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemMgr.GetAllItems().ToList())
      });
    }

    /// <summary>
    /// Privacy page for logged-in and non-logged-in users.
    /// </summary>
    [AllowAnonymous]
    public ActionResult Privacy()
    {
      const string PRIVACY_PAGE_TITLE = "Privacy en veiligheid";
      UserManager userManager = new UserManager();
      return View(new BaseViewModel()
      {
        PageTitle = PRIVACY_PAGE_TITLE,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
      });
    }

    /// <summary>
    /// FAQ page for logged-in and non-logged-in users.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    public ActionResult Faq()
    {
      const string FAQ_PAGE_TITLE = "Vraag en antwoord";
      UserManager userManager = new UserManager();
      return View(new BaseViewModel()
      {
        PageTitle = FAQ_PAGE_TITLE,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
      });
    }
  }
}