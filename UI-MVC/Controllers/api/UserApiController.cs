using System.Net;
using System.Web.Http;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This controller is used for doing api-calls to work with users.
	/// </summary>
	public class UserApiController : ApiController
	{
		private IUserManager userManager;
		private ISubscriptionManager SubManager;
		private IdentityUserManager userManagerIdentity;

		/// <summary>
		/// Toggles a Subscription for a specific user (based on wether the subscriptions exists or not).
		/// </summary>
		[HttpPost]
		[Route("api/ToggleSubscribe/{itemId}")]
		[Authorize]
		public IHttpActionResult ToggleSubscribe(int itemId)
		{
			SubManager = new SubscriptionManager();
			string userId = User.Identity.GetUserId();
			SubManager.ToggleSubscription(userId, itemId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Updates password from logged-in user.
		/// </summary>
		[HttpPost]
		[Route("api/User/UpdateAccount")]
		public async Task<IHttpActionResult> UpdateAccount(SettingsViewModel model)
		{
			userManagerIdentity = HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
			User user = await userManagerIdentity.FindByIdAsync(User.Identity.GetUserId());

			if (await userManagerIdentity.CheckPasswordAsync(user, model.Password))
			{
				await userManagerIdentity.ChangePasswordAsync(User.Identity.GetUserId(), model.Password, model.PasswordNew);
				return StatusCode(HttpStatusCode.NoContent);
			}

			return StatusCode(HttpStatusCode.NotAcceptable);
		}

		/// <summary>
		/// Updates general profile info from logged-in user.
		/// </summary>
		[HttpPost]
		[Route("api/User/UpdateProfile")]
		public IHttpActionResult UpdateProfile(SettingsViewModel model)
		{
			userManager = new UserManager();

			Area area = userManager.GetArea(model.SelectedAreaId);
			User user = userManager.ChangeUserBasicInfo(User.Identity.GetUserId(), model.Firstname, model.Lastname, model.Gender, model.DateOfBirth, area);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Updates alert preferences from logged-in user.
		/// </summary>
		[HttpPost]
		[Route("api/User/UpdateAlerts")]
		public IHttpActionResult UpdateAlerts(SettingsViewModel model)
		{
			userManager = new UserManager();
			User user = userManager.ChangeUserAlerts(User.Identity.GetUserId(), model.AlertsViaWebsite, model.AlertsViaEmail, model.WeeklyReviewViaEmail);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Toggle user account acitivity
		/// </summary>
		[HttpPost]
		[Route("api/Admin/ToggleAccountActivity/{userId}")]
		public IHttpActionResult ToggleAccountActivity(string userId)
		{
			if (userId == null) return BadRequest("No userId given");
			userManager = new UserManager();
			userManager.ChangeUserAccount(userId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Change user role.
		/// </summary>
		[HttpPost]
		[Route("api/Admin/ChangeRole/{userId}")]
		public IHttpActionResult ChangeRole(string userId, [FromBody]string roleName)
		{
			if (roleName.Equals("User") || roleName.Equals("Admin") || roleName.Equals("SuperAdmin"))
			{
				userManager = new UserManager();
				string currentRole = userManager.GetRole(userId).Name;

				userManagerIdentity = HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
				userManagerIdentity.RemoveFromRole(userId, currentRole);
				userManagerIdentity.AddToRole(userId, roleName);

				return StatusCode(HttpStatusCode.NoContent);
			}

			return StatusCode(HttpStatusCode.NotAcceptable);
		}
	}
}