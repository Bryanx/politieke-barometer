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
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net;
using Microsoft.Owin.Security;

namespace BAR.UI.MVC.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		#region Identity

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();
			//var status = CheckRCaptcha();

			//if (status == false)
			//{
			//  return View();
			//}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View(model);
			}
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			RegisterViewModel registerViewModel = new RegisterViewModel();
		 // ViewBag.RoleList = new SelectList(UserManager.GetAllRoles(), "Name", "Name");
			return View(registerViewModel);
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();
			UserManager userManager = HttpContext.GetOwinContext().GetUserManager<UserManager>();

			if (ModelState.IsValid)
			{
				var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.Firstname, LastName = model.Lastname, Gender = model.Gender, DateOfBirth = model.DateOfBirth };
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					//Add claim to user
					//await UserManager.AddClaimAsync(user.Id, new Claim(ClaimTypes.DateOfBirth, model.DateOfBirth.ToShortDateString()));
					//Send an email with this link
					string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
					var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
					await userManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
					//Assign Role to user    
					//await UserManager.AddToRoleAsync(user.Id, model.UserRoles);
					//Login
					await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					return RedirectToAction("Index", "Home");
				}
				AddErrors(result);
			}
			return View(model);
		}

    //
    // POST: /Account/LogOff
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff()
    {
      IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
      authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      return RedirectToAction("Index", "Home");
    }

    #endregion

    public ISubscriptionManager subManager = new SubscriptionManager();
		/// <summary>
		/// Dashboard of the user
		/// </summary>
		public ActionResult Index()
		{
			var id = User.Identity.GetUserId();
			ViewBag.Id = id;
			UserSubscribedPeopleDTO model = GetUserSubscribedModel(id);
			return View("Dashboard","~/Views/Shared/Layouts/_MemberLayout.cshtml", model);
		}

		public ActionResult Settings() {
			var id = User.Identity.GetUserId();
			ViewBag.Id = id;
			UserSubscribedPeopleDTO model = GetUserSubscribedModel(id);
			return View("Settings","~/Views/Shared/Layouts/_MemberLayout.cshtml", model);
		}
		
	
		private UserSubscribedPeopleDTO GetUserSubscribedModel(string id) {
			UserManager userManager = HttpContext.GetOwinContext().GetUserManager<UserManager>();
			User user = userManager.GetUser(id);
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

		#region Helpers

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

		#endregion
	}
}