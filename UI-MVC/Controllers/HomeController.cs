using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Helpers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;
using BAR.BL.Domain.Core;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the homepage.
	/// </summary>
	public class HomeController : LanguageController
	{
		private const string INDEX_PAGE_TITLE = "Politieke Barometer";
		private IItemManager itemManager;
		private IUserManager userManager;
		private ISubplatformManager subplatformManager;

		/// <summary>
		/// Landing page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Index()
		{

			//Get hold of subplatformID we saved in the [SubplatformCheck] attribute
			int subPlatformId = (int)RouteData.Values["SubPlatformID"];

			userManager = new UserManager();
			itemManager = new ItemManager();
			subplatformManager = new SubplatformManager();

			// -------- Making WeeklyReviewModel --------
			// Getting trending items
			List<Item> weeklyTrendings = itemManager.GetMostTrendingItems(subPlatformId, 4, true).ToList();

			// Getting PersonViewModels
			List<PersonViewModel> weeklyPersonViewModels = new List<PersonViewModel>();
			itemManager.GetMostTrendingItemsForType(subPlatformId, ItemType.Person, 4, true).ForEach(item => weeklyPersonViewModels.Add(Mapper.Map(item, new PersonViewModel())));
			for (int i = 0; i < weeklyPersonViewModels.Count; i++)
			{
				weeklyPersonViewModels[i].Item = Mapper.Map(weeklyTrendings[i], new ItemDTO());
				weeklyPersonViewModels[i].SocialMediaNames = itemManager.GetPersonWithDetails(weeklyTrendings[i].ItemId).SocialMediaNames;

			}

			// ------- Making TopTrending -------
			// Getting trending items
			List<Item> trendings = itemManager.GetMostTrendingItems(subPlatformId, 3).ToList();

			// Getting personViewmodels
			List<PersonViewModel> trendingPersonViewModels = new List<PersonViewModel>();
			itemManager.GetMostTrendingItemsForType(subPlatformId, ItemType.Person, 3).ForEach(item => trendingPersonViewModels.Add(Mapper.Map(item, new PersonViewModel())));
			for (int i = 0; i < trendingPersonViewModels.Count; i++)
			{
				trendingPersonViewModels[i].Item = Mapper.Map(trendings[i], new ItemDTO());
				trendingPersonViewModels[i].SocialMediaNames = itemManager.GetPersonWithDetails(trendings[i].ItemId).SocialMediaNames;
			}

			//Assembling the view
			return View(new ItemViewModel
			{
				PageTitle = INDEX_PAGE_TITLE,
				User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
				Items = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemManager.GetAllItems().Where(item => item.SubPlatform.SubPlatformId.Equals(subPlatformId)).ToList()),
				TopTrendingPersonViewModels = trendingPersonViewModels,
				TopTrendingitems = trendings,
				WeeklyReviewModel = new WeeklyReviewModel
				{
					WeeklyPersonViewModels = weeklyPersonViewModels,
					WeeklyItems = weeklyTrendings
				}
			});
		}

		/// <summary>
		/// Privacy page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Privacy()
		{
			userManager = new UserManager();

			//Assembling the view
			return View(new BaseViewModel()
			{
				PageTitle = Resources.PrivacyAndSafety,
				User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
			});
		}

		/// <summary>
		/// FAQ page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Faq()
		{
			userManager = new UserManager();
			subplatformManager = new SubplatformManager();

			IEnumerable<Question> questions = subplatformManager.GetAllQuestions();//Assembling the view
			return View(new BaseViewModel()
			{
				PageTitle = Resources.QuestionAndAnswer,
				User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
			});
		}

		/// <summary>
		/// Saves the language preferences in a cookie.
		/// </summary>
		public ActionResult SetCulture(string culture)
		{
			// Validate input
			culture = LanguageHelper.GetImplementedCulture(culture);

			// Save culture in a cookie
			HttpCookie cookie = Request.Cookies["_culture"];
			if (cookie != null)
				cookie.Value = culture;   // update cookie value
			else
			{
				cookie = new HttpCookie("_culture")
				{
					Value = culture,
					Expires = DateTime.Now.AddYears(1)
				};
			}
			Response.Cookies.Add(cookie);

			return Redirect(Request.UrlReferrer.ToString());
		}

		/// <summary>
		/// Returns a javascript object with language resources.
		/// This way local resources can be used in javascript files.
		/// </summary>
		public ActionResult GetResources()
		{
			Response.ContentType = "text/javascript";
			return View();
		}


		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult HeaderImage()
		{
			subplatformManager = new SubplatformManager();

			Customization customization = subplatformManager.GetCustomization((int)RouteData.Values["SubPlatformID"]);
			if (customization.HeaderImage == null) return null;
			return new FileContentResult(customization.HeaderImage, "image/jpeg");
		}

		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult LogoImage()
		{
			subplatformManager = new SubplatformManager();

			Customization customization = subplatformManager.GetCustomization((int)RouteData.Values["SubPlatformID"]);
			if (customization.HeaderImage == null) return null;
			return new FileContentResult(customization.LogoImage, "image/jpeg");
		}

		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult DarkLogoImage()
		{
			subplatformManager = new SubplatformManager();

			Customization customization = subplatformManager.GetCustomization((int)RouteData.Values["SubPlatformID"]);
			if (customization.HeaderImage == null) return null;
			return new FileContentResult(customization.DarkLogoImage, "image/jpeg");
		}
	}
}