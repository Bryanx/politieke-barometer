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
        private IUserManager userManager = new UserManager();            
        private ISubscriptionManager subMgr = new SubscriptionManager();

        /// <summary>
        /// Item page for logged-in and non-logged-in users.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Index() {
            IList<ItemDTO> people = Mapper.Map(itemMgr.GetAllPeople(), new List<ItemDTO>());
            IEnumerable<Subscription> subs = subMgr.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
            foreach (ItemDTO item in people) {
                foreach (var sub in subs) {
                    if (sub.SubscribedItem.ItemId == item.ItemId) item.Subscribed = true;
                }
            }
            return View("Index", 
                new ItemViewModel() {
                PageTitle = INDEX_PAGE_TITLE,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                Items = people
            });
        }

        /// <summary>
        /// Detailed item page for logged-in and non-logged-in users.
        /// </summary>
        public ActionResult Details(int id) {
            IEnumerable<Item> subs = subMgr.GetSubscribedItemsForUser(User.Identity.GetUserId());
            Item item = itemMgr.GetItem(id);
            Item subbedItem = subs.FirstOrDefault(i => i.ItemId == item.ItemId);
            return View("Details", 
                new PersonViewModel() {
                PageTitle = item.Name,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                Person = Mapper.Map(item, new ItemDTO()),
                Subscribed = subbedItem != null
            });
        }
    }
}