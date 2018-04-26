using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the organisation-page.
	/// </summary>
	public class OrganisationController : LanguageController
	{
		private ISubscriptionManager subManager;
		private IItemManager itemManager;
		private IUserManager userManager;

		/// <summary>
		/// Organisation page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Index()
		{
      //Get hold of subplatformID we received
      int subPlatformID = (int)RouteData.Values["SubPlatformID"];

      subManager = new SubscriptionManager();
			itemManager = new ItemManager();
			userManager = new UserManager();

			IList<ItemDTO> people = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemManager.GetAllOrganisations().Where(org => org.SubPlatform.SubPlatformId == subPlatformID).ToList());
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
					PageTitle = Resources.AllParties,
					User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
					Items = people
				});
		}
	}
}