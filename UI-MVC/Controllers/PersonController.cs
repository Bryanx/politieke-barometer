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

namespace BAR.UI.MVC.Controllers {
    public class PersonController : Controller {
        
        private const string INDEX_PAGE_TITLE = "Politici-overzicht";
        private IItemManager itemMgr = new ItemManager();
        private UserManager userManager = new UserManager();
        
        [AllowAnonymous]
        public ActionResult Index() {
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

            ItemViewModel itemViewModel = new ItemViewModel() {
                PageTitle = INDEX_PAGE_TITLE,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                People = personen
            };
            return View("Index", itemViewModel);
        }

        // GET: Default/Details/5 (Specific person page)
        public ActionResult Details(int id) {
            Item item = itemMgr.GetItem(id);
            PersonViewModel personViewModel = new PersonViewModel() {
                PageTitle = item.Name,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                Person = Mapper.Map(item, new ItemDTO())
            };
            return View("Details", personViewModel);
        }
    }
}