using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
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
		private ISubplatformManager subplatformManager;

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
			subplatformManager = new SubplatformManager();

			IList<ItemDTO> people = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemManager.GetAllOrganisationsForSubplatform(subPlatformID).ToList());
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
					PageTitle = subplatformManager.GetCustomization(subPlatformID).OrganisationsAlias,
					User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
					Items = people
				});
		}

		/// <summary>
		/// Detailed organisation page for logged-in and non-logged-in users.
		/// </summary>
		[SubPlatformDataCheck]
		public ActionResult Details(int id)
		{
			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

			Item org = itemManager.GetOrganisationWithDetails(id);
			if (org == null) return HttpNotFound();

			Item subbedItem = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId())
				.FirstOrDefault(i => i.ItemId == org.ItemId);

			OrganisationViewModel organisationViewModel = Mapper.Map(org, new OrganisationViewModel());
			organisationViewModel.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;
			organisationViewModel.Organisation = Mapper.Map(org, new ItemDTO());
			organisationViewModel.Subscribed = subbedItem != null;
			organisationViewModel.MemberList = GetOrgMembers(org);

			//Log visit actitivy
			new SubplatformManager().LogActivity(ActivityType.VisitActitiy);

			//Assembling the view
			return View(organisationViewModel);
		}

		/// <summary>
		/// Gives back all the members of a specific organisation
		/// </summary>
		private List<PersonViewModel> GetOrgMembers(Item org)
		{
			List<Person> persons = itemManager.GetAllPersons()
				.Where(person => person.Organisation.ItemId == org.ItemId)
				.OrderByDescending(person => person.NumberOfMentions)
				.ToList();

			List<PersonViewModel> personViewModels = Mapper.Map(persons, new List<PersonViewModel>());
			for (int i = 0; i < persons.Count; i++)
			{
				personViewModels[i].Item = Mapper.Map(persons[i], new ItemDTO());
			}
			return personViewModels;
		}
	}
}