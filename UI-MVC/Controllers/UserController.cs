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
        /// <summary>
        /// This method is called every time  when a
        /// user wants to see his/her alers
        /// </summary>
        /// <param name="userId"></param>
        public ActionResult Index(int userId)
        {
            ISubscriptionManager subManager = new SubscriptionManager();
            IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(userId);
            return View(alertsToShow);
        }
    }
}