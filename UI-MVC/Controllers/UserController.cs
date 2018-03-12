using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Managers;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index(int userId)
        {
            ISubscriptionManager subManager = new SubscriptionManager();
            IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(userId);
            return View(alertsToShow);
        }
    }
}