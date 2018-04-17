using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;
using BAR.UI.MVC.Attributes;
using BAR.BL.Domain;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the person-page.
	/// </summary>
	public class PersonController : LanguageController
	{
		private IItemManager itemManager;
		private IUserManager userManager;
		private ISubscriptionManager subManager;
		private ISubplatformManager subplatformManager;

		/// <summary>
		/// Item page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		[SubPlatformCheck]
		public ActionResult Index()
		{
			//Assign the right subplatform
			string subPlatformName = (string) RouteData.Values["SubPlatform"];
			subplatformManager = new SubplatformManager();
			SubPlatform subplatform = subplatformManager.GetSubPlatform(subPlatformName);

			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

      IList<ItemDTO> people = null;

      if (subplatform == null)
			{
				//Do generic version for no specific subplatform
				people = Mapper.Map(itemManager.GetAllPersons(), new List<ItemDTO>());
				
			} else
			{
        //Return platformspecific data
        List<Item> peopleList = itemManager.GetAllPersonsForSubplatform(subPlatformName).ToList();
				people = Mapper.Map(itemManager.GetAllPersonsForSubplatform(subplatform.Name), new List<ItemDTO>());

			}
			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
			foreach (ItemDTO item in people)
			{
				foreach (var sub in subs)
				{
					if (sub.SubscribedItem.ItemId == item.ItemId) item.Subscribed = true;
				}
			}

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
		public ActionResult Details(int id)
		{
			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

			IEnumerable<Item> subs = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId());
			Item item = itemManager.GetItem(id);
			Item subbedItem = subs.FirstOrDefault(i => i.ItemId == item.ItemId);

			//Assembling the view
			return View("Details",
				new PersonViewModel()
				{
					PageTitle = item.Name,
					User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
					Person = Mapper.Map(item, new ItemDTO()),
					Subscribed = subbedItem != null
				});
		}
	}
}