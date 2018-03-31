using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers {
    public class HomeController : Controller {
        
        IItemManager itemMgr = new ItemManager();
        
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
            return View("Index", "~/Views/Shared/Layouts/_HomeLayout(temp).cshtml", personen.AsEnumerable());
        }
    }
}