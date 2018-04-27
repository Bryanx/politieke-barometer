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
		private IItemManager itemManager;
		private ISubplatformManager platformManager;
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
			return View(new SubPlatformManagement
			{
				PageTitle = Resources.SubPlatformManagement,
				User = userManager.GetUser(User.Identity.GetUserId())
			});
		}

		/// <summary>
		/// Adminmanagement page of the SuperAdmin.
		/// </summary>
		public ActionResult AdminManagement()
		{
			IdentityUserManager identityUserManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
			userManager = new UserManager();

			IEnumerable<User> users = userManager.GetAllUsers();
			List<string> currentRoles = new List<string>();
			for (int i = 0; i < users.Count(); i++)
			{
				currentRoles.Add(identityUserManager.GetRoles(users.ElementAt(i).Id).FirstOrDefault());
			}

			//Assembling the view
			ViewBag.CurrentRoles = currentRoles;
			EditUserViewModel vm = new EditUserViewModel()
			{
				User = userManager.GetUser(User.Identity.GetUserId()),
				PageTitle = Resources.AdminManagement,
				Users = users,
			};
			FillViewModels(vm);
			return View(vm);
		}

		/// <summary>
		/// Fills a viewmodel with selectionlists. the selectionlist will
		/// be shown in a dropdownmenu.
		/// </summary>
		private void FillViewModels(EditUserViewModel vm)
		{
			userManager = new UserManager();

			vm.AdminRoles = userManager.GetAllRoles().Select(x => new SelectListItem
			{
				Value = x.Id,
				Text = x.Name,
			}).OrderBy(x => x.Text);
			vm.UserRoles = userManager.GetAllRoles().Where(r => r.Name == "Admin" || r.Name == "User")
				.Select(x => new SelectListItem
				{
					Value = x.Id,
					Text = x.Name,
				}).OrderBy(x => x.Text);
		}
	}
}