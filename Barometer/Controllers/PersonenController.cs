using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Barometer.Models;

namespace Barometer.Controllers {
    public class PersonenController : Controller {
        
        // GET: Default
        public ActionResult Index() {
            return View(Persoon.getPersonen());
        }

        // GET: Default/Details/5
        public ActionResult Details(int id) {
            return View(Persoon.getPersonen().First(m => m.Id == id));
        }
    }
}