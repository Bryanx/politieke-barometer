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
using BAR.UI.MVC.App_GlobalResources;
using BAR.BL.Domain.Core;

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
		private ISubplatformManager platformManager;

		/// <summary>
		/// Dashboard page of admin.
		/// </summary>
		public ActionResult Index()
		{
			return HttpNotFound();
		}

		/// <summary>
		/// Page management page of admin.
		/// </summary>
		public ActionResult PageManagement()
		{
			userManager = new UserManager();
			platformManager = new SubplatformManager();

			//Map viewmodel
			Customization platform = platformManager.GetCustomization(2);

			CustomizationViewModel vm = Mapper.Map(platform, new CustomizationViewModel());
			vm.User = userManager.GetUser(User.Identity.GetUserId());
			vm.PageTitle = Resources.PageManagement;

			//Assembling the view
			return View(vm);
		}

		/// <summary>
		/// Item management page of admin.
		/// </summary>
		public ActionResult ItemManagement()
		{
      //Get hold of subplatformID we received
      int subPlatformID = (int)RouteData.Values["SubPlatformID"];

      itemManager = new ItemManager();
			userManager = new UserManager();

      //Assembling the view
      return View(new ItemViewModels.ItemViewModel()
			{
				User = userManager.GetUser(User.Identity.GetUserId()),
				PageTitle = Resources.ItemManagement,
				Items = Mapper.Map(itemManager.GetAllItems().Where(item => item.SubPlatform.SubPlatformId == subPlatformID), new List<ItemDTO>())
			});
		}

		/// <summary>
		/// User management page of admin.
		/// </summary>
		public ActionResult UserManagement()
		{
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
				PageTitle = Resources.UserManagement,
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

    [HttpPost]
    public ActionResult UploadJson([Bind(Exclude = "jsonFile")]ItemViewModels.ItemViewModel model)
    {
      //Get hold of subplatformID we received
      int subPlatformID = (int)RouteData.Values["SubPlatformID"];

      itemManager = new ItemManager();

      if (Request.Files.Count > 0)
      {
        HttpPostedFileBase pfb = Request.Files["jsonFile"];
        string json = itemManager.ConvertPfbToString(pfb);
        itemManager.ImportJson(json, subPlatformID);
      }
      return RedirectToAction("ItemManagement", "Admin");
    }

		[HttpPost]
		public ActionResult UploadThemes([Bind(Exclude = "jsonFile")]ItemViewModels.ItemViewModel model)
		{
			//Get hold of subplatformID we received
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];

			itemManager = new ItemManager();

			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase pfb = Request.Files["jsonFile"];
				string json = itemManager.ConvertPfbToString(pfb);
				itemManager.ImportThemes(json, subPlatformID);
			}
			return RedirectToAction("ItemManagement", "Admin");
		}
	}
}