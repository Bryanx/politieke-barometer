using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BAR.BL;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;

namespace BAR.UI.MVC.Controllers {
    public class PersonController : Controller {
        
        IItemManager itemMgr = new ItemManager();
        

        [AllowAnonymous]
        public ActionResult Index() {
            List<PersonDTO> personen = new List<PersonDTO>();
            foreach (Item item in itemMgr.GetAllItems()) {
                personen.Add(new PersonDTO() {
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
            }

            if (User.Identity.IsAuthenticated) {
                UserManager userManager = new UserManager();
                UserSubscribedPeopleDTO usr = new UserSubscribedPeopleDTO() {
                    User = userManager.GetUser(User.Identity.GetUserId()),
                    People = personen
                };
                return View("Index", "~/Views/Shared/Layouts/_MemberLayout.cshtml", usr);
            } else {
                UserSubscribedPeopleDTO usr = new UserSubscribedPeopleDTO() {
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
                UserSubscribedPeopleDTO usr = new UserSubscribedPeopleDTO() {
                    User = userManager.GetUser(User.Identity.GetUserId()),
                    People = persoon
                };
                return View("Details", "~/Views/Shared/Layouts/_MemberLayout.cshtml", usr);
            } else {
                UserSubscribedPeopleDTO usr = new UserSubscribedPeopleDTO() {
                    User = null,
                    People = persoon
                };
                return View("Details", "~/Views/Shared/Layouts/_VisitorLayout.cshtml", usr);
            }
        }
    }
}