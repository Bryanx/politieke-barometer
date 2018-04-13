using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers {
    public class OrganisationController : Controller {
        
        private const string INDEX_PAGE_TITLE = "Partij-overzicht";
        IItemManager itemMgr = new ItemManager();
        private UserManager userManager = new UserManager();
        
        /// <summary>
        /// Organisation page for logged-in and non-logged-in users.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Index() {
            ISubscriptionManager subMgr = new SubscriptionManager();
            IList<ItemDTO> people = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemMgr.GetAllOrganisations().ToList());
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
    }
}