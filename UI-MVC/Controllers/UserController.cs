using System;
using BAR.BL.Controllers;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAR.BL.Domain.Items;
using BAR.UI.MVC.Models;
using Subscription = BAR.BL.Domain.Users.Subscription;

namespace BAR.UI.MVC.Controllers
{
	public class UserController : Controller
	{
		IUserManager usrMgr = new UserManager();
		public ISubscriptionManager subManager = new SubscriptionManager();
		/// <summary>
		/// Dashboard of the user
		/// </summary>
		public ActionResult Index(int id)
		{
			ViewBag.Id = id;
			UserSubscribedPeopleDTO model = GetUserSubscribedModel(id);
			return View("Dashboard","~/Views/Shared/Layouts/_MemberLayout.cshtml", model);
		}

		public ActionResult Settings(int id) {
			ViewBag.Id = id;
			UserSubscribedPeopleDTO model = GetUserSubscribedModel(id);
			return View("Settings","~/Views/Shared/Layouts/_MemberLayout.cshtml", model);
		}
		
	
		private UserSubscribedPeopleDTO GetUserSubscribedModel(int id) {
			User user = usrMgr.GetUser(id);
			//TODO: These next statements should be in a method in BL
			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(id);
			List<Item> items = subs.Select(s => s.SubscribedItem).ToList();
			List<PersonDTO> people = new List<PersonDTO>();
			foreach (Item item in items) {
				people.Add(new PersonDTO() {
					ItemId = item.ItemId,
					Name = item.Name,
					CreationDate = item.CreationDate,
					LastUpdated = item.LastUpdated,
					Description = item.Description,
					NumberOfFollowers = item.NumberOfFollowers,
					TrendingPercentage = Math.Floor(item.TrendingPercentage),
					NumberOfMentions = item.NumberOfMentions,
					Baseline = item.Baseline
				});
			}
			return new UserSubscribedPeopleDTO() {
				User = user,
				People = people
			};
		}
	}
}