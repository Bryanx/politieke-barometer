using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BAR.UI.MVC.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// This method is called every time  when a
        /// user wants to see his/her alers
        /// </summary>
        public ActionResult Index(int userId)
        {
            ISubscriptionManager subManager = new SubscriptionManager();
            IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(userId);
            return View(alertsToShow);
        }
    }
}