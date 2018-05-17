using System;
using AutoMapper;
using BAR.BL.Domain.Core;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
		public ActionResult Index() {
			userManager = new UserManager();
			platformManager = new SubplatformManager();

			return View(new BaseViewModel() {
				PageTitle = Resources.AdminDashboard,
				User = userManager.GetUser(User.Identity.GetUserId()),
				Customization = platformManager.GetCustomization((int)RouteData.Values["SubPlatformID"])
			});
		}

		/// <summary>
		/// Page management page of admin.
		/// </summary>
		public ActionResult PageManagement()
		{
			userManager = new UserManager();
			platformManager = new SubplatformManager();

			//Map viewmodel
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];
			Customization custom = platformManager.GetCustomization(subPlatformID);

			CustomizationViewModel vm = Mapper.Map(custom, new CustomizationViewModel());
			vm.User = userManager.GetUser(User.Identity.GetUserId());
			vm.PageTitle = Resources.PageManagement;
			vm.Customization = platformManager.GetCustomization((int)RouteData.Values["SubPlatformID"]);


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
			platformManager = new SubplatformManager();

			//Assembling the view
			return View(new ItemViewModels.ItemViewModel()
			{
				User = userManager.GetUser(User.Identity.GetUserId()),
				PageTitle = Resources.ItemManagement,
				Items = Mapper.Map(itemManager.GetAllItems().Where(item => item.SubPlatform.SubPlatformId == subPlatformID), new List<ItemDTO>()),
				Customization = platformManager.GetCustomization(subPlatformID)

			});
		}

		/// <summary>
		/// User management page of admin.
		/// </summary>
		public ActionResult UserManagement()
		{
			userManager = new UserManager();
			platformManager = new SubplatformManager();

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
				Users = users,
				Customization = platformManager.GetCustomization((int)RouteData.Values["SubPlatformID"])
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
		public ActionResult UploadThemes([Bind(Exclude = "jsonFileThemes")]ItemViewModels.ItemViewModel model)
		{
			//Get hold of subplatformID we received
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];

			itemManager = new ItemManager();

			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase pfb = Request.Files["jsonFileThemes"];
				string json = itemManager.ConvertPfbToString(pfb);
				itemManager.ImportThemes(json, subPlatformID);
			}
			return RedirectToAction("ItemManagement", "Admin");
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
		public ActionResult CreatePerson()
		{
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];

			itemManager = new ItemManager();
			platformManager = new SubplatformManager();
			SubPlatform subplatform = platformManager.GetSubPlatform(subPlatformID);

			Person p = (Person)itemManager.AddItem(ItemType.Person, "Maarten Jorens");
			p.SubPlatform = subplatform;

			return RedirectToAction("Details", "Person", new { id = p.ItemId });
		}

		/// <summary>
		/// POST
		/// Changes headerimage of a subplatform.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult HeaderImage([Bind(Exclude = "WebsiteHeader")]CustomizationViewModel model)
		{
			platformManager = new SubplatformManager();

			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase headerImgFile = Request.Files["WebsiteHeader"];
				int subPlatformID = (int)RouteData.Values["SubPlatformID"];
				platformManager.ChangeSubplatformHeaderImage(subPlatformID, headerImgFile);
			}
			return RedirectToAction("PageManagement", "Admin");
		}

		/// <summary>
		/// POST
		/// Changes headerimage of a subplatform.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult WebsiteLogo([Bind(Exclude = "WebsiteLogo")]CustomizationViewModel model)
		{
			platformManager = new SubplatformManager();

			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase logoImgFile = Request.Files["WebsiteLogo"];
				int subPlatformID = (int)RouteData.Values["SubPlatformID"];
				platformManager.ChangeSubplatformLogo(subPlatformID, logoImgFile);
			}
			return RedirectToAction("PageManagement", "Admin");
		}

		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult HeaderImage()
		{
			platformManager = new SubplatformManager();

			Customization customization = platformManager.GetCustomization((int)RouteData.Values["SubPlatformID"]);
			if (customization.HeaderImage == null) return null;
			return new FileContentResult(customization.HeaderImage, "image/jpeg");
		}

		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult LogoImage()
		{
			platformManager = new SubplatformManager();

			Customization customization = platformManager.GetCustomization((int)RouteData.Values["SubPlatformID"]);
			if (customization.HeaderImage == null) return null;
			return new FileContentResult(customization.LogoImage, "image/jpeg");
		}
	}
}