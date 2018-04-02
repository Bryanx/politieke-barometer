using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;

namespace BAR.UI.MVC.Controllers {
    public class OrganisationController : Controller {
        
        private const string INDEX_PAGE_TITLE = "Partij-overzicht";
        IItemManager itemMgr = new ItemManager();
        
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

                if (item is Organisation) {
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
    }
}