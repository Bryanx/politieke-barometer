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
using WebGrease.Css.Extensions;

namespace BAR.UI.MVC.Controllers {
    public class PersonController : Controller {
        
        IItemManager itemMgr = new ItemManager();
        
        [AllowAnonymous]
        public ActionResult Index() {
            ISubscriptionManager subMgr = new SubscriptionManager();
            IEnumerable<Subscription> subs = subMgr.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
            List<PersonDTO> personen = new List<PersonDTO>();
            foreach (Item item in itemMgr.GetAllItems()) {
                bool subbed = false;
                foreach (var sub in subs) {
                    if (sub.SubscribedItem.ItemId == item.ItemId) subbed = true;
                }
                personen.Add(new PersonDTO() {
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

            if (User.Identity.IsAuthenticated) {
                UserManager userManager = new UserManager();
                UserViewModel usr = new UserViewModel() {
                    User = userManager.GetUser(User.Identity.GetUserId()),
                    People = personen
                };
                return View("Index", "~/Views/Shared/Layouts/_MemberLayout.cshtml", usr);
            } else {
                UserViewModel usr = new UserViewModel() {
                    User = null,
                    People = personen
                };
                return View("Index", "~/Views/Shared/Layouts/_VisitorLayout.cshtml", usr);
            }
        }

        // GET: Default/Details/5 (Specific person page)
        public ActionResult Details(int id) {
            List<PersonDTO> persoon = new List<PersonDTO>();
            Item item = itemMgr.GetItem(id);
            persoon.Add(new PersonDTO() {
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
            if (User.Identity.IsAuthenticated) {
                UserManager userManager = new UserManager();
                UserViewModel uvm = new UserViewModel() {
                    User = userManager.GetUser(User.Identity.GetUserId()),
                    People = persoon
                };
                return View("Details", "~/Views/Shared/Layouts/_MemberLayout.cshtml", uvm);
            } else {
                UserViewModel uvm = new UserViewModel() {
                    User = null,
                    People = persoon
                };
                return View("Details", "~/Views/Shared/Layouts/_VisitorLayout.cshtml", uvm);
            }
        }
    }
}