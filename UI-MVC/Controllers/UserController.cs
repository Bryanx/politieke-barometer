using BAR.BL.Controllers;
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
		/// user wants to see his/her alers.
		/// </summary>
		public ActionResult Index(int userId)
		{
			SysController sys = new SysController();
			sys.DetermineTrending();
			sys.GenerateAlerts();

			ISubscriptionManager subManager = new SubscriptionManager();
			IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(userId);

			ViewBag.Alerts = alertsToShow;

			return View(ViewBag.Alerts);
		}
	}
}