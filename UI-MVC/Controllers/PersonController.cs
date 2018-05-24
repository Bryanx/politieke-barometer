using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
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
	/// This controller is used for managing the person-pages.
	/// </summary>
	public class PersonController : LanguageController
	{
		private IItemManager itemManager;
		private IUserManager userManager;
		private ISubscriptionManager subManager;

		/// <summary>
		/// Item page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Index()
		{
			//Get hold of subplatformID we received
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];

			itemManager = new ItemManager();
			userManager = new UserManager();
			subManager = new SubscriptionManager();

			List<Person> persons = itemManager.GetAllPersonsForSubplatform(subPlatformID).ToList();

			//Return platformspecific data
			PersonViewModels personViewModels = new PersonViewModels();
			personViewModels.Persons = Mapper.Map(persons, personViewModels.Persons);
			personViewModels.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;

			List<Person> allPersons = itemManager.GetAllPersonsForSubplatform(subPlatformID).ToList();
			List<ItemDTO> items = Mapper.Map(allPersons, new List<ItemDTO>());
			for (int i = 0; i < items.Count; i++)
			{
				personViewModels.Persons[i].Item = items[i];
				personViewModels.Persons[i].Item.Picture = persons[i].Picture;

			}

			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
			personViewModels.Persons.Where(person => subs.Any(s => s.SubscribedItem.ItemId == person.Item.ItemId)).ForEach(item => item.Subscribed = true);

			personViewModels.Persons.ForEach(person => person.RankNumberOfMentions = CalculateRankNumberOfMentions(person.Item.NumberOfMentions, allPersons));
			personViewModels.Persons.ForEach(person => person.RankTrendingPercentage = CalculateRankTrendingPercentage(person.Item.ItemId, allPersons));

			//By default person pages are ordered by trending percentage.
			personViewModels.Persons = personViewModels.Persons.OrderByDescending(person => person.Item.TrendingPercentage).ToList();

			//Assembling the view
			return View("Index", personViewModels);

		}

		/// <summary>
		/// Calculates the most trending items by trending percentage
		/// </summary>
		private int CalculateRankTrendingPercentage(int itemId, List<Person> items = null)
		{
			if (items == null)
			{
				items = new ItemManager().GetAllPersonsForSubplatform((int)RouteData.Values["SubPlatformID"]).ToList();
			}
			return items.OrderByDescending(i => i.TrendingPercentage).ToList()
				.FindIndex(p => p.ItemId == itemId) + 1;
		}

		/// <summary>
		/// Calculates the most number of mentions for the items
		/// </summary>
		private int CalculateRankNumberOfMentions(int numberOfMentions, List<Person> items = null)
		{
			if (items == null)
			{
				items = new ItemManager().GetAllPersonsForSubplatform((int)RouteData.Values["SubPlatformID"]).ToList();
			}
			return items.OrderByDescending(i => i.NumberOfMentions).ToList()
				.FindIndex(p => p.NumberOfMentions == numberOfMentions) + 1;
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

			Item item = itemManager.GetPersonWithDetails(id);
			if (item == null) return HttpNotFound();

			Item subbedItem = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId())
				.FirstOrDefault(i => i.ItemId == item.ItemId);

			PersonViewModel personViewModel = Mapper.Map(item, new PersonViewModel());

			personViewModel.PageTitle = item.Name;
			personViewModel.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;
			personViewModel.Item = Mapper.Map(item, new ItemDTO());
			personViewModel.Subscribed = subbedItem != null;

			personViewModel.RankNumberOfMentions = CalculateRankNumberOfMentions(personViewModel.Item.NumberOfMentions);
			personViewModel.RankTrendingPercentage = CalculateRankTrendingPercentage(personViewModel.Item.ItemId);
			personViewModel.PeopleFromSameOrg = GetPeopleFromSameOrg(personViewModel.Item.ItemId);

			//Log visit activity
			new SubplatformManager().LogActivity(ActivityType.VisitActitiy);

			//Assembling the view
			return View(personViewModel);
		}

		/// <summary>
		/// Gives back the persons that are in the same organisation
		/// </summary>
		private List<PersonViewModel> GetPeopleFromSameOrg(int itemId)
		{
			try
			{
				int orgId = itemManager.GetPersonWithDetails(itemId).Organisation.ItemId;
				List<Person> persons = itemManager.GetAllPersons()
					.Where(p => p.Organisation.ItemId == orgId)
					.Where(p => p.ItemId != itemId) //except the current person
					.OrderByDescending(p => p.NumberOfMentions)
					.Take(6).ToList();
				List<PersonViewModel> personViewModels = Mapper.Map(persons, new List<PersonViewModel>());
				for (int i = 0; i < persons.Count; i++)
				{
					personViewModels[i].Item = Mapper.Map(persons[i], new ItemDTO());
				}
				return personViewModels;
#pragma warning disable CS0168 // Variable is declared but never used
			}
			catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				List<PersonViewModel> personViewModels = Mapper.Map(new List<Person>(), new List<PersonViewModel>());
				return personViewModels;
			}

		}

		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult Picture(int itemId)
		{
			itemManager = new ItemManager();

			Item item = itemManager.GetItem(itemId);
			if (item.Picture == null) return null;
			return new FileContentResult(item.Picture, "image/jpeg");
		}

		/// <summary>
		/// POST
		/// Changes profile picture of logged-in user.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ChangePicture([Bind(Exclude = "Picture")]ItemDTO model)
		{
			itemManager = new ItemManager();

			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase poImgFile = Request.Files["Picture"];
				itemManager.ChangePicture(model.ItemId, poImgFile);
			}

			return RedirectToAction("Details", "Person", new { id = model.ItemId });
		}
	}
}