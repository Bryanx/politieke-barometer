using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System;
using Microsoft.AspNet.Identity.Owin;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the functionality
	/// of the superadmins
	/// </summary>
	[Authorize(Roles = "SuperAdmin")]
	public class SuperAdminController : LanguageController
	{
		private IUserManager userManager;
    private IDataManager dataManager;

		/// <summary>
		/// Sourcemanagement page of the SuperAdmin.
		/// </summary>
		public ActionResult GeneralManagement()
		{
			userManager = new UserManager();
      dataManager = new DataManager();
      //Assembling the view
      return View(new SourceManagement
      {
        PageTitle = Resources.GeneralManagement,
        User = userManager.GetUser(User.Identity.GetUserId()),
        Sources = dataManager.GetAllSources()
			});
		}

		/// <summary>
		/// Platformmanagement page of the SuperAdmin.
		/// </summary>
		public ActionResult PlatformManagement()
		{
			userManager = new UserManager();

			//Assembling the view
			return View(new BaseViewModel
			{
				PageTitle = Resources.SubPlatformManagement,
				User = userManager.GetUser(User.Identity.GetUserId())
			});
		}w
	}
}