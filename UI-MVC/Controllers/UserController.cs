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
using System.Configuration;
using System.Security.Claims;
using BAR.BL;

namespace BAR.UI.MVC.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		#region Identity

		//
		// GET: /User/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "User");
      }

      ViewBag.ReturnUrl = returnUrl;
			return View();
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
					ModelState.AddModelError("", "Ongeldige inlogpoging.");
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
      RegisterViewModel registerViewModel = new RegisterViewModel();
			return View(registerViewModel);
		}

		//
		// POST: /User/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "User");
      }

      SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();
			IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();

			if (ModelState.IsValid)
			{
				var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.Firstname, LastName = model.Lastname, Gender = model.Gender, DateOfBirth = model.DateOfBirth, AlertsViaWebsite = true};
				var result = await userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					//Send an email with this link
					string code = await userManager.GenerateEmailConfirmationTokenAsync(user.Id);
					var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
					await userManager.SendEmailAsync(user.Id, "Bevestig je registratie", "Bevestig je registratie door <a href=\"" + callbackUrl + "\">hier</a> te klikken.");
					//Assign Role to user    
					await userManager.AddToRoleAsync(user.Id, "User");
					//Login
					await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					return RedirectToAction("Index", "Home");
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
    // GET: /User/VerifyCode
    [AllowAnonymous]
    public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
    {
      SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();

      // Require that the user has already logged in via username/password or external login
      if (!await signInManager.HasBeenVerifiedAsync())
      {
        return View("Error");
      }
      return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }

    //
    // POST: /User/VerifyCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
    {
      SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToLocal(model.ReturnUrl);
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.Failure:
        default:
          ModelState.AddModelError("", "Ongeldige code.");
          return View(model);
      }
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
      return View();
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
        var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        await userManager.SendEmailAsync(user.Id, "Reset wachtwoord", "Je kan je wachtwoord resetten door <a href=\"" + callbackUrl + "\">hier</a> te klikken.");
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
      return View();
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
      return View();
    }

    //
    // POST: /User/ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult ExternalLogin(string provider, string returnUrl)
    {
      // Request a redirect to the external login provider
      return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "User", new { ReturnUrl = returnUrl }));
    }

    //
    // GET: /User/SendCode
    [AllowAnonymous]
    public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
    {
      SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();
      IdentityUserManager userManager = HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();

      var userId = await signInManager.GetVerifiedUserIdAsync();
      if (userId == null)
      {
        return View("Error");
      }
      var userFactors = await userManager.GetValidTwoFactorProvidersAsync(userId);
      var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
      return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }

    //
    // POST: /User/SendCode
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> SendCode(SendCodeViewModel model)
    {
      SignInManager signInManager = HttpContext.GetOwinContext().Get<SignInManager>();

      if (!ModelState.IsValid)
      {
        return View();
      }

      // Generate the token and send it
      if (!await signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
      {
        return View("Error");
      }
      return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
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

      // Sign in the user with this external login provider if the user already has a login
      var result = await signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
      switch (result)
      {
        case SignInStatus.Success:
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
          return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, Firstname = firstname, Lastname = lastname });
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

        var user = new User{ UserName = model.Email, Email = model.Email, FirstName = model.Firstname, LastName = model.Lastname, Gender = model.Gender, DateOfBirth = model.DateOfBirth };
        var result = await userManager.CreateAsync(user);

        if (result.Succeeded)
        {
          result = await userManager.AddLoginAsync(user.Id, info.Login);
     
          //Assign Role to user Here      
          await userManager.AddToRoleAsync(user.Id, "User");

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
      return View();
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
			UserViewModel model = GetUserSubscribedModel(id);
			return View("Dashboard","~/Views/Shared/Layouts/_MemberLayout.cshtml", model);
		}

		public ActionResult Settings() {
			var id = User.Identity.GetUserId();
			ViewBag.Id = id;
			UserViewModel model = GetUserSubscribedModel(id);
			return View("Settings","~/Views/Shared/Layouts/_MemberLayout.cshtml", model);
		}
		
	
		private UserViewModel GetUserSubscribedModel(string id) {
			IUserManager userManager = new UserManager();
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
					LastUpdated = item.LastUpdatedInfo,
					Description = item.Description,
					NumberOfFollowers = item.NumberOfFollowers,
					TrendingPercentage = Math.Floor(item.TrendingPercentage),
					NumberOfMentions = item.NumberOfMentions,
					Baseline = item.Baseline,
				});
			}
			return new UserViewModel() {
				User = user,
				People = people
			};
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