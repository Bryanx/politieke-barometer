using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace BAR.UI.MVC.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Dashboard() {
            ViewBag.Message = "Your applicationn description page.";

            return View("~/Views/User/Dashboard.cshtml","~/Views/Shared/_MemberLayout.cshtml");
        }
    }
}