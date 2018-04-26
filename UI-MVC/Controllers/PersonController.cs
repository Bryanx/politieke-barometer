using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;
using BAR.UI.MVC.Attributes;
using BAR.BL.Domain;
using BAR.BL.Domain.Core;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the person-pages.
	/// </summary>
	public class PersonController : LanguageController
	{
		private IItemManager itemManager;
		private IUserManager userManager;
		private ISubscriptionManager subManager;
		private IWidgetManager widgetManager;

		/// <summary>
		/// Item page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Index()
		{
			//Get hold of subplatformID we received
			int subPlatformID = (int) RouteData.Values["SubPlatformID"];

			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

			//Return platformspecific data
			IList<ItemDTO> people = null;
			people = Mapper.Map(itemManager.GetAllPersonsForSubplatform(subPlatformID), new List<ItemDTO>());

			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
			people.Where(p => subs.Any(s => s.SubscribedItem.ItemId == p.ItemId)).ForEach(dto => dto.Subscribed = true);

			//Assembling the view
			return View("Index",
				new ItemViewModel()
				{
					PageTitle = Resources.AllPoliticians,
					User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
					Items = people
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
			widgetManager = new WidgetManager();

			IEnumerable<Item> subs = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId());
			Item item = itemManager.GetItem(id);
			Item subbedItem = subs.FirstOrDefault(i => i.ItemId == item.ItemId);

			PersonViewModel personViewModel =
				new PersonViewModel() {
					PageTitle = item.Name,
					User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
					Person = Mapper.Map(item, new ItemDTO()),
					Subscribed = subbedItem != null,
				};
			//Assembling the view
			return View("Details", personViewModel);
		}
	}
}