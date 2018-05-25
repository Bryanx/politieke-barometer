using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the theme-pages
	/// </summary>
	public class ThemeController : LanguageController
	{
		private IItemManager itemManager;
		private IUserManager userManager;
		private ISubscriptionManager subManager;
		private ISubplatformManager subplatformManager;

		/// <summary>
		/// Theme overview page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Index()
		{
			//Get hold of subplatformID we received
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];

			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();
			subplatformManager = new SubplatformManager();

			//Return platformspecific data
			IList<ItemDTO> themes = null;
			themes = Mapper.Map(itemManager.GetAllThemes().Where(theme => theme.SubPlatform.SubPlatformId == subPlatformID), new List<ItemDTO>());

			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
			themes.Where(theme => subs.Any(sub => sub.SubscribedItem.ItemId == theme.ItemId)).ForEach(dto => dto.Subscribed = true);

			//Assembling the view
			return View("Index",
				new ItemViewModel()
				{
					PageTitle = subplatformManager.GetCustomization(subPlatformID).ThemesAlias,
					User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
					Items = themes
				});
		}

		/// <summary>
		/// Detailed item page for logged-in and non-logged-in users.
		/// </summary>
		[SubPlatformDataCheck]
		public ActionResult Details(int id)
		{
			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

			Item requestedItem = itemManager.GetThemeWithDetails(id);
			Theme theme = itemManager.GetThemeWithDetails(id);

			if (requestedItem == null) return HttpNotFound();

			Item subbedItem = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId())
				.FirstOrDefault(item => item.ItemId == id);

			ThemeViewModel themeViewModel = Mapper.Map(theme, new ThemeViewModel());

			themeViewModel.PageTitle = theme.Name;
			themeViewModel.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;
			themeViewModel.Theme = Mapper.Map(theme, new ItemDTO());
			themeViewModel.Subscribed = subbedItem != null;
			themeViewModel.Keywords = theme.Keywords.ToList();

			//Log visit activity
			new SubplatformManager().LogActivity(ActivityType.VisitActitiy);

			//Assembling the view
			return View(themeViewModel);
		}
	}
}