using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using AutoMapper;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for admins and superadmins.
	/// </summary>
	[Authorize(Roles = "Admin, SuperAdmin")]
	public class AdminController : LanguageController
	{
		private IUserManager userManager;
		private IItemManager itemManager;

		/// <summary>
		/// Dashboard page of admin.
		/// </summary>
		public ActionResult Index()
		{
			const string ADMIN_DASHBOARD_PAGE_TITLE = "Admin Dashboard";
			return HttpNotFound();
		}

		/// <summary>
		/// Page management page of admin.
		/// </summary>
		public ActionResult PageManagement()
		{
			const string PAGE_MANAGEMENT_PAGE_TITLE = "Pagina's beheren";
			userManager = new UserManager();

			//Assembling the view
			return View(new BaseViewModel()
			{
				User = userManager.GetUser(User.Identity.GetUserId()),
				PageTitle = PAGE_MANAGEMENT_PAGE_TITLE
			});
		}

		/// <summary>
		/// Item management page of admin.
		/// </summary>
		public ActionResult ItemManagement()
		{
			const string ITEM_MANAGEMENT_PAGE_TITLE = "Items beheren";
			itemManager = new ItemManager();
			userManager = new UserManager();

			//Assembling the view
			return View(new ItemViewModels.ItemViewModel()
			{
				User = userManager.GetUser(User.Identity.GetUserId()),
				PageTitle = ITEM_MANAGEMENT_PAGE_TITLE,
				Items = Mapper.Map(itemManager.GetAllItems(), new List<ItemDTO>())
			});
		}

		/// <summary>
		/// User management page of admin.
		/// </summary>
		public ActionResult UserManagement()
		{
			const string USER_MANAGEMENT_PAGE_TITLE = "Gebruikers beheren";
			userManager = new UserManager();

			//Get Roles
			IdentityUserManager identityUserManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
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
				PageTitle = USER_MANAGEMENT_PAGE_TITLE,
				Users = users
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