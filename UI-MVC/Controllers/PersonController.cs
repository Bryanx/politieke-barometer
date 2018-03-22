using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers {
    public class PersonController : Controller {
        
        IItemManager itemMgr = new ItemManager();
        
        // GET: Default (Person overview page)
        public ActionResult Index() {
            List<PersonDTO> personen = new List<PersonDTO>();
            foreach (Item item in itemMgr.getAllItems()) {
                personen.Add(new PersonDTO() {
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