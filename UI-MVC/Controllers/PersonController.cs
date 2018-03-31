using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAR.BL;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using MvcBreadCrumbs;
using WebGrease.Css.Extensions;

namespace BAR.UI.MVC.Controllers {
    [BreadCrumb]
    public class PersonController : Controller {
        
        private const string INDEX_PAGE_TITLE = "Politici-overzicht";
        IItemManager itemMgr = new ItemManager();
        
        [AllowAnonymous]
        [BreadCrumb(Clear = true, Label = INDEX_PAGE_TITLE)]
        public ActionResult Index() {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            ISubscriptionManager subMgr = new SubscriptionManager();
            IEnumerable<Subscription> subs = subMgr.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
            List<ItemDTO> personen = new List<ItemDTO>();
            foreach (Item item in itemMgr.GetAllItems()) {
                bool subbed = false;
                foreach (var sub in subs) {
                    if (sub.SubscribedItem.ItemId == item.ItemId) subbed = true;
                }

                if (item is Person) {
                    personen.Add(new ItemDTO() {
                        ItemId = item.ItemId,
                        Name = item.Name,
                        CreationDate = item.CreationDate,
                        LastUpdated = item.LastUpdatedInfo,
                        Description = item.Description,
                        NumberOfFollowers = item.NumberOfFollowers,
                        TrendingPercentage = Math.Floor(item.TrendingPercentage),
                        NumberOfMentions = item.NumberOfMentions,
                        Baseline = item.Baseline,
                        Subscribed = subbed
                    });
                }
            }

            UserManager userManager = new UserManager();
            ItemViewModel itemViewModel = new ItemViewModel() {
                PageTitle = INDEX_PAGE_TITLE,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                People = personen
            };
            return View("Index", "~/Views/Shared/Layouts/_MemberLayout.cshtml", itemViewModel);
        }

        // GET: Default/Details/5 (Specific person page)
        [BreadCrumb]
        public ActionResult Details(int id) {
            Item item = itemMgr.GetItem(id);
            BreadCrumb.SetLabel(item.Name);
            List<ItemDTO> persoon = new List<ItemDTO>();
            persoon.Add(new ItemDTO() {
                ItemId = item.ItemId,
                Name = item.Name,
                CreationDate = item.CreationDate,
                LastUpdated = item.LastUpdatedInfo,
                Description = item.Description,
                NumberOfFollowers = item.NumberOfFollowers,
                TrendingPercentage = Math.Floor(item.TrendingPercentage),
                NumberOfMentions = item.NumberOfMentions,
                Baseline = item.Baseline
            });
            UserManager userManager = new UserManager();
            ItemViewModel itemViewModel = new ItemViewModel() {
                PageTitle = persoon.First().Name,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                People = persoon
            };
            return View("Details", "~/Views/Shared/Layouts/_MemberLayout.cshtml", itemViewModel);
        }
    }
}