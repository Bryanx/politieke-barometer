﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

			Item item = itemManager.GetPersonWithDetails(id);

			if (item == null) return HttpNotFound();

			Item subbedItem = subManager.GetSubscribedItemsForUser(User.Identity.GetUserId())
				.FirstOrDefault(i => i.ItemId == item.ItemId);

			PersonViewModel personViewModel = Mapper.Map(item, new PersonViewModel());
			
			personViewModel.PageTitle = item.Name;
			personViewModel.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;
			personViewModel.Person = Mapper.Map(item, new ItemDTO());
			personViewModel.Subscribed = subbedItem != null;
			                             
			//Assembling the view
			return View(personViewModel);
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