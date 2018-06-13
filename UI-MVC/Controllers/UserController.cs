using System;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAR.BL.Domain.Items;
using BAR.UI.MVC.Models;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using BAR.UI.MVC.App_GlobalResources;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net;
using Microsoft.Owin.Security;
using AutoMapper;
using BAR.BL.Domain.Widgets;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
	/// <summary>
	/// This controller is used for managing the users.
	/// </summary>
	[Authorize]
	public class UserController : LanguageController
	{
		private IUserManager userManager;
		private ISubscriptionManager subManager;
		private ISubplatformManager subplatformManager;

		#region Identity
		
		// GET: /User/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "User");
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(new LoginViewModel());
		}

		//
		// POST: /User/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "User");
			}

			SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
				shouldLockout: true);
			switch (result)
			{
				case SignInStatus.Success:
				//Log logging activity
				new SubplatformManager().LogActivity(ActivityType.LoginActivity);
				return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
				return View("Lockout");
				case SignInStatus.RequiresVerification:
				return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
				ModelState.AddModelError("", Resources.LoginFailed);
				return View(model);
			}
		}

		//
		// GET: /User/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "User");
			}

			RegisterViewModel registerViewModel = new RegisterViewModel
			{
				DateOfBirth = DateTime.Now
			};
			return View(registerViewModel);
		}

		//
		// POST: /User/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
			SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "User");
			}

			if (ModelState.IsValid)
			{
				var user = new User
				{
					UserName = model.Email,
					Email = model.Email,
					FirstName = model.Firstname,
					LastName = model.Lastname,
					Gender = model.Gender,
					DateOfBirth = model.DateOfBirth,
					IsActive = true,
					AlertsViaWebsite = true
				};
				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					//Send an email with this link
					string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
					var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, code = code },
						protocol: Request.Url.Scheme);
					await userManager.SendEmailAsync(user.Id, Resources.ConfirmAccount,
						"<a href=\"" + callbackUrl + "\">" + Resources.ConfirmAccountClickingHere + "</a>");
					//Assign Role to user    
					await userManager.AddToRoleAsync(user.Id, "User");

					//Log activity
					new SubplatformManager().LogActivity(ActivityType.RegisterActivity);

					//Login
					await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					return RedirectToAction("Index", "User");
				}

				AddErrors(result);
			}

			return View(model);
		}

		//
		// POST: /User/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
			authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /User/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();

			if (userId == null || code == null)
			{
				return View("Error");
			}

			var result = await userManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		//
		// GET: /User/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View(new ForgotPasswordViewModel());
		}

		//
		// POST: /User/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();

			if (ModelState.IsValid)
			{
				var user = await userManager.FindByNameAsync(model.Email);
				if (user == null || !(await userManager.IsEmailConfirmedAsync(user.Id)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return View("ForgotPasswordConfirmation");
				}

				// Send an email with this link
				string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
				var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id, code = code },
					protocol: Request.Url.Scheme);
				await userManager.SendEmailAsync(user.Id, Resources.ResetPassword,
					"<a href=\"" + callbackUrl + "\">" + Resources.ResetPasswordByClickingHere + "</a>");
				return RedirectToAction("ForgotPasswordConfirmation", "User");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /User/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View(new ForgotPasswordViewModel());
		}

		//
		// GET: /User/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			return code == null ? View("Error") : View();
		}

		//
		// POST: /User/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await userManager.FindByNameAsync(model.Email);
			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction("ResetPasswordConfirmation", "User");
			}

			var result = await userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("ResetPasswordConfirmation", "User");
			}

			AddErrors(result);
			return View();
		}

		//
		// GET: /User/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View(new ResetPasswordViewModel());
		}

		//
		// POST: /User/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider,
				Url.Action("ExternalLoginCallback", "User", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /User/ExternalLoginCallback
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
			SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();

			var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			//Get information from the social provider best on available claims
			var firstname = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:first_name").Value;
			var lastname = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:last_name").Value;
			var id = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:id").Value;

			//Get profile picure as byte array
			var webClient = new WebClient();
			var photoUrl = String.Format("https://graph.facebook.com/{0}/picture?type=large", id);
			byte[] imageData = webClient.DownloadData(photoUrl);

			// Sign in the user with this external login provider if the user already has a login
			var result = await signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
				//Log logging activity
				new SubplatformManager().LogActivity(ActivityType.LoginActivity);
				return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
				return View("Lockout");
				case SignInStatus.RequiresVerification:
				return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
				// If the user does not have an account, then prompt the user to create an account
				ViewBag.ReturnUrl = returnUrl;
				ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
				return View("ExternalLoginConfirmation",
					new ExternalLoginConfirmationViewModel
					{
						Email = loginInfo.Email,
						Firstname = firstname,
						Lastname = lastname,
						DateOfBirth = DateTime.Now,
						ImageData = imageData
					});
			}
		}

		//
		// POST: /User/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
			IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
			SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();

			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "User");
			}

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await authenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				var user = new User
				{
					UserName = model.Email,
					Email = model.Email,
					FirstName = model.Firstname,
					LastName = model.Lastname,
					Gender = model.Gender,
					DateOfBirth = model.DateOfBirth,
					ProfilePicture = model.ImageData
				};

				var result = await userManager.CreateAsync(user);

				if (result.Succeeded)
				{
					result = await userManager.AddLoginAsync(user.Id, info.Login);

					//Assign Role to user Here      
					await userManager.AddToRoleAsync(user.Id, "SuperAdmin");

					//log activity
					new SubplatformManager().LogActivity(ActivityType.RegisterActivity);

					if (result.Succeeded)
					{
						await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}

				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// GET: /User/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View(new ExternalLoginConfirmationViewModel());
		}

		#endregion

		/// <summary>
		/// Dashboard of the user.
		/// </summary>
		public ActionResult Index()
		{
			string userId = User.Identity.GetUserId();
			PersonViewModels itemViewModel = GetPersonViewModel(userId);
			itemViewModel.PageTitle = Resources.Dashboard;

			//Assebling the view
			return View("Dashboard", itemViewModel);
		}

		/// <summary>
		/// Settings page of the user.
		/// </summary>
		public ActionResult Settings()
		{
			userManager = new UserManager();

			User user = userManager.GetUser(User.Identity.GetUserId());
			var areas = userManager.GetAreas().Select(area => new SelectListItem
			{
				Value = area.AreaId.ToString(),
				Text = area.Residence,
			}).OrderBy(area => area.Text);

			//Assembling the view
			SettingsViewModel settingsViewModel = new SettingsViewModel
			{
				User = user,
				Firstname = user.FirstName,
				Lastname = user.LastName,
				Gender = user.Gender,
				DateOfBirth = user.DateOfBirth ?? DateTime.Now,
				Areas = areas,
				AlertsViaWebsite = user.AlertsViaWebsite,
				AlertsViaEmail = user.AlertsViaEmail,
				WeeklyReviewViaEmail = user.WeeklyReviewViaEmail,
				PageTitle = Resources.Settings
			};
			return View(settingsViewModel);
		}

		/// <summary>
		/// POST
		/// Changes profile picture of logged-in user.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Settings([Bind(Exclude = "ProfilePicture")]SettingsViewModel model)
		{
			userManager = new UserManager();

			if (Request.Files.Count > 0)
			{
				HttpPostedFileBase poImgFile = Request.Files["ProfilePicture"];
				userManager.ChangeProfilePicture(User.Identity.GetUserId(), poImgFile);
			}
			return RedirectToAction("Settings", "User");
		}

		/// <summary>
		/// Returns image of byte array.
		/// </summary>
		public FileContentResult ProfilePicture()
		{
			userManager = new UserManager();

			User user = userManager.GetUser(User.Identity.GetUserId());
			if (user.ProfilePicture == null) return null;
			return new FileContentResult(user.ProfilePicture, "image/jpeg");
		}
		
		/// <summary>
		/// Returns profile picture of a specific user.
		/// </summary>
		public FileContentResult ProfilePictureForUser(string id)
		{
			userManager = new UserManager();

			User user = userManager.GetUser(id);
			if (user.ProfilePicture == null) return null;
			return new FileContentResult(user.ProfilePicture, "image/jpeg");
		}

		/// <summary>
		/// Gets user model with all his subscribed items.
		/// </summary>
		private PersonViewModels GetPersonViewModel(string id)
		{
			ItemManager itemManager = new ItemManager();
			subManager = new SubscriptionManager();
			userManager = new UserManager();
			subplatformManager = new SubplatformManager();
			
			int subPlatformID = (int) RouteData.Values["SubPlatformID"];

			PersonViewModels personViewModels = new PersonViewModels();
			personViewModels.Persons = Mapper.Map(itemManager.GetAllPersonsForSubplatform(subPlatformID), personViewModels.Persons);
			
			itemManager.GetAllOrganisationsForSubplatform(subPlatformID).ForEach(item => {	
				personViewModels.Persons.Add(new PersonViewModel() {
					Item = Mapper.Map(item, new ItemDTO())
				});
			});

			itemManager.GetAllThemesForSubplatform(subPlatformID).ForEach(item => {
				personViewModels.Persons.Add(new PersonViewModel() {
					Item = Mapper.Map(item, new ItemDTO()),
					OrganisationId = -1
				});
			});
			
			personViewModels.PageTitle = subplatformManager.GetCustomization(subPlatformID).PersonsAlias;
			personViewModels.User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null;

			List<ItemDTO> items = Mapper.Map(itemManager.GetAllPersonsForSubplatform(subPlatformID), new List<ItemDTO>());
			for(int i = 0; i < items.Count; i++) {
				personViewModels.Persons[i].Item = items[i];
			}

			IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
			personViewModels.Persons = personViewModels.Persons.Where(person => subs.Any(sub => sub.SubscribedItem.ItemId == person.Item.ItemId)).ToList();
			personViewModels.Persons.ForEach(person => person.Subscribed = true);

			return personViewModels;
		}

		/// <summary>
		/// Gives back a partial view of the weekly review
		/// </summary>
		public ActionResult UserWeeklyReview()
		{
			int subPlatformID = (int)RouteData.Values["SubPlatformID"];

			// Building WeeklyReviewModel
			WidgetManager widgetManager = new WidgetManager();
			ItemManager itemManager = new ItemManager();
			userManager = new UserManager();
			string userId = User.Identity.GetUserId();
			ICollection<int> defaults = new List<int>();
			int countPerson = 0;
			int countOrg = 0;
			int countTheme = 0;

			// -------- Making WeeklyReviewModel --------
			// Getting trending items for user and all items
			List<Item> weeklyTrendings = new List<Item>();

			itemManager.GetMostTrendingItemsForUserAndItemType(subPlatformID, userId, ItemType.Person, 4, true).OrderBy(item => item.TrendingPercentage).Reverse().ForEach(item => weeklyTrendings.Add(item));
			if (weeklyTrendings.Count < 4)
			{
				List<Item> trendings = itemManager.GetMostTrendingItemsForType(subPlatformID, ItemType.Person, 4, true).ToList();
				
				while (weeklyTrendings.Count < 4)
				{
					List<int> ids = new List<int>();
					weeklyTrendings.ForEach(i => ids.Add(i.ItemId));
					if (ids.Contains(trendings.First().ItemId))
					{
						trendings.RemoveAt(0);
					}
					else
					{
						weeklyTrendings.Add(trendings.First());
						trendings.RemoveAt(0);
						countPerson++;
					}
				}
			}

			weeklyTrendings.OrderBy(item => item.TrendingPercentage);

			// Getting PersonViewModels for Persons
			List<PersonViewModel> weeklyPersonViewModels = new List<PersonViewModel>();
			weeklyTrendings.ForEach(item => weeklyPersonViewModels.Add(Mapper.Map(item, new PersonViewModel())));
			List<Person> details = new List<Person>();
			weeklyTrendings.ForEach(i => details.Add(itemManager.GetPersonWithDetails(i.ItemId)));
			for (int i = 0; i < weeklyPersonViewModels.Count; i++)
			{
				weeklyPersonViewModels[i].Item = Mapper.Map(weeklyTrendings[i], new ItemDTO());
				weeklyPersonViewModels[i].SocialMediaNames = details[i].SocialMediaNames;
				weeklyPersonViewModels[i].OrganisationId = details[i].Organisation.ItemId;
				weeklyPersonViewModels[i].OrganisationName = details[i].Organisation.Name;

			}

			itemManager.GetMostTrendingItemsForUserAndItemType(subPlatformID, userId, ItemType.Organisation, 4, true).OrderBy(item => item.TrendingPercentage).Reverse().ForEach(item => weeklyTrendings.Add(item));
			if (weeklyTrendings.Count < 8)
			{
				List<Item> trendings = itemManager.GetMostTrendingItemsForType(subPlatformID, ItemType.Organisation, 4, true).ToList();
				while (weeklyTrendings.Count < 8)
				{
					List<int> ids = new List<int>();
					weeklyTrendings.ForEach(i => ids.Add(i.ItemId));
					if (ids.Contains(trendings.First().ItemId))
					{
						trendings.RemoveAt(0);
					}
					else
					{
						weeklyTrendings.Add(trendings.First());
						trendings.RemoveAt(0);
						countOrg++;
					}
				}
			}

			itemManager.GetMostTrendingItemsForUserAndItemType(subPlatformID, userId, ItemType.Theme, 4, true).OrderBy(item => item.TrendingPercentage).Reverse().ForEach(item => weeklyTrendings.Add(item));
			if (weeklyTrendings.Count < 12)
			{
				List<Item> trendings = itemManager.GetMostTrendingItemsForType(subPlatformID, ItemType.Theme, 4, true).ToList();
				while (weeklyTrendings.Count < 12)
				{
					List<int> ids = new List<int>();
					weeklyTrendings.ForEach(i => ids.Add(i.ItemId));
					if (ids.Contains(trendings.First().ItemId))
					{
						trendings.RemoveAt(0);
					}
					else
					{
						weeklyTrendings.Add(trendings.First());
						trendings.RemoveAt(0);
						countTheme++;
					}
				}
			}

			// Making widgets
			List<Widget> widgets = new List<Widget>();

			foreach (Item item in weeklyTrendings)
			{
				IEnumerable<Widget> widgetsToAdd = widgetManager.GetAllWidgetsWithAllDataForItem(item.ItemId)
					.Where(widget => widget.PropertyTags
						.Any(proptag => proptag.Name.ToLower().Equals("mentions")))
					.AsEnumerable();
				if (widgetsToAdd != null) widgets.AddRange(widgetsToAdd);
			}
			
			// Making defaults list
			defaults.Add(countPerson);
			defaults.Add(countOrg);
			defaults.Add(countTheme);


			WeeklyReviewModel weeklyReviewModel = new WeeklyReviewModel
			{
				WeeklyPersonViewModels = weeklyPersonViewModels,
				WeeklyItems = weeklyTrendings,
				Widgets = widgets,
				User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
				Defaults = defaults,
				PageTitle = Resources.WeeklyReview
			};

			return View(weeklyReviewModel);
		}

		#region Helpers

		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return RedirectToAction("Index", "Home");
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}

				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}

		#endregion
	}
}