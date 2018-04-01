using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;

namespace BAR.UI.MVC.Controllers {
    public class HomeController : Controller {
        private const string INDEX_PAGE_TITLE = "Politieke Barometer";
        IItemManager itemMgr = new ItemManager();
        
        [AllowAnonymous]
        public ActionResult Index() {
            List<ItemDTO> personen = new List<ItemDTO>();
            foreach (Item item in itemMgr.GetAllItems()) {
                personen.Add(new ItemDTO() {
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
            UserManager userManager = new UserManager();
            ItemViewModel itemViewModel = new ItemViewModel() {
                PageTitle = INDEX_PAGE_TITLE,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                People = personen
            };
            return View("Index","~/Views/Shared/Layouts/_HomeLayout(temp).cshtml", itemViewModel);
        }

    }
}