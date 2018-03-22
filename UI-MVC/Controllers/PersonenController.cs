using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers {
    public class PersonenController : Controller {
        
        IItemManager itemMgr = new ItemManager();
        
        // GET: Default (Person overview page)
        public ActionResult Index() {
            List<PersoonDTO> personen = new List<PersoonDTO>();
            foreach (Item item in itemMgr.getAllItems()) {
                personen.Add(new PersoonDTO() {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    CreationDate = item.CreationDate,
                    LastUpdated = item.LastUpdated,
                    Description = item.Description,
                    NumberOfFollowers = item.NumberOfFollowers,
                    TrendingPercentage = item.TrendingPercentage,
                    NumberOfMentions = item.NumberOfMentions,
                    Baseline = item.Baseline
                });
            }
            return View(personen.AsEnumerable());
        }

        // GET: Default/Details/5 (Specific person page)
        public ActionResult Details(int id) {
            return View(itemMgr.GetItem(id));
        }
    }
}