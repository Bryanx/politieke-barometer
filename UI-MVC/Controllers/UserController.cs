using BAR.BL.Controllers;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BAR.UI.MVC.Controllers
{
	public class UserController : Controller
	{
		IUserManager usrMgr = new UserManager();
		/// <summary>
		/// Dashboard of the user
		/// </summary>
		public ActionResult Index(int id)
		{
			ViewBag.Id = id;
			User user = usrMgr.GetUser(id);
			return View("Dashboard","~/Views/Shared/_MemberLayout.cshtml", user);
		}
	}
}