using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the homepage.
	/// </summary>
	public class HomeController : Controller
	{
		private const string INDEX_PAGE_TITLE = "Politieke Barometer";
		private const string PRIVACY_PAGE_TITLE = "Privacy en veiligheid";
		private const string FAQ_PAGE_TITLE = "Vraag en antwoord";
		private IItemManager itemManager;
		private IUserManager userManager;

		/// <summary>
		/// Landing page for logged-in and non-logged-in users.
		/// </summary>
		[AllowAnonymous]
		public ActionResult Index()
		{
			userManager = new UserManager();
			itemManager = new ItemManager();

			//Assembling the view
			return View(new ItemViewModel()
			{
				PageTitle = INDEX_PAGE_TITLE,
				User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
				Items = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemManager.GetAllItems().ToList())
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
				PageTitle = PRIVACY_PAGE_TITLE,
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

			//Assembling the view
			return View(new BaseViewModel()
			{
				PageTitle = FAQ_PAGE_TITLE,
				User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
			});
		}
	}
}