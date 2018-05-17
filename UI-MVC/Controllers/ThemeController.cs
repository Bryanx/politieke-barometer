using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
		private IWidgetManager widgetManager;

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

			//Return platformspecific data
			IList<ItemDTO> themes = null;
			themes = Mapper.Map(itemManager.GetAllThemes().Where(theme => theme.SubPlatform.SubPlatformId == subPlatformID), new List<ItemDTO>());

			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
			themes.Where(p => subs.Any(s => s.SubscribedItem.ItemId == p.ItemId)).ForEach(dto => dto.Subscribed = true);

			//Assembling the view
			return View("Index",
				new ItemViewModel()
				{
					PageTitle = Resources.AllPoliticians,
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
			//** TESTING **//
			new WidgetManager().GetWidgetsForWeeklyReview();

			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

			Item item = itemManager.GetThemeWithDetails(id);
			Theme theme = itemManager.GetThemeWithDetails(id);

			if (item == null) return HttpNotFound();

			Item subbedItem = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId())
				.FirstOrDefault(i => i.ItemId == item.ItemId);

			ThemeViewModel themeViewModel = Mapper.Map(theme, new ThemeViewModel());

			themeViewModel.PageTitle = theme.Name;
			themeViewModel.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;
			themeViewModel.Theme = Mapper.Map(theme, new ItemDTO());
			themeViewModel.Subscribed = subbedItem != null;
			themeViewModel.Keywords = theme.Keywords.ToList();

			//Assembling the view
			return View(themeViewModel);
		}
	}
}