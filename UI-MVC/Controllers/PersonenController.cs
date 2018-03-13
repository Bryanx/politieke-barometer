using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UI.MVC.Models;

namespace UI.MVC.Controllers {
    public class PersonenController : Controller {
        
        // GET: Default (Person overview page)
        public ActionResult Index() {
            return View(Persoon.getPersonen());
        }

        // GET: Default/Details/5 (Specific person page)
        public ActionResult Details(int id) {
            return View(Persoon.getPersonen().First(m => m.Id == id));
        }
    }
}