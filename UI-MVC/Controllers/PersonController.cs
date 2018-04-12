using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
  public class PersonController : Controller
  {
    /// <summary>
    /// Item page for logged-in and non-logged-in users.
    /// </summary>
    [AllowAnonymous]
    public ActionResult Index()
    {
      IItemManager itemMgr = new ItemManager();
      UserManager userManager = new UserManager();
      const string INDEX_PAGE_TITLE = "Politici-overzicht";
      ISubscriptionManager subMgr = new SubscriptionManager();
      IList<ItemDTO> people = Mapper.Map(itemMgr.GetAllPeople(), new List<ItemDTO>());
      IEnumerable<Subscription> subs = subMgr.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
      foreach (ItemDTO item in people)
      {
        foreach (var sub in subs)
        {
          if (sub.SubscribedItem.ItemId == item.ItemId) item.Subscribed = true;
        }
      }
      return View("Index", new ItemViewModel()
      {
        PageTitle = INDEX_PAGE_TITLE,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
        Items = people
      });
    }

    /// <summary>
    /// Detailed item page for logged-in and non-logged-in users.
    /// </summary>
    public ActionResult Details(int id)
    {
      IItemManager itemMgr = new ItemManager();
      UserManager userManager = new UserManager();
      Item item = itemMgr.GetItem(id);
      return View("Details", new PersonViewModel()
      {
        PageTitle = item.Name,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
        Person = Mapper.Map(item, new ItemDTO())
      });
    }
  }
}