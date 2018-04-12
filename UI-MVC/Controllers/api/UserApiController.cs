using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BAR.BL.Controllers;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Host.SystemWeb;
using BAR.BL;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace BAR.UI.MVC.Controllers.api
{
  public class UserApiController : ApiController
  {
    /// <summary>
    /// Creates a Subscription for logged-in user.
    /// </summary>
    [HttpPost]
    [Route("api/Subscribe/{itemId}")]
    public IHttpActionResult CreateSubscription(int itemId)
    {
      ISubscriptionManager SubManager = new SubscriptionManager();
      string userId = User.Identity.GetUserId();
      SubManager.CreateSubscription(userId, itemId);
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Removes a Subscription from logged-in user.
    /// </summary>
    [HttpDelete]
    [Route("api/User/Subscription/{itemId}/Delete")]
    public IHttpActionResult DeleteSubscription(int itemId)
    {
      ISubscriptionManager subManager = new SubscriptionManager();
      IEnumerable<Subscription> subs = subManager.GetSubscriptionsWithItemsForUser(User.Identity.GetUserId());
      Subscription sub = subs.Single(s => s.SubscribedItem.ItemId == itemId);
      subManager.RemoveSubscription(sub.SubscriptionId);
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Updates password from logged-in user.
    /// </summary>
    [HttpPost]
    [Route("api/User/UpdateAccount")]
    public async Task<IHttpActionResult> UpdateAccount(SettingsViewModel model)
    {
      IdentityUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
      User user = await userManager.FindByIdAsync(User.Identity.GetUserId());
      if (await userManager.CheckPasswordAsync(user, model.Password))
      {
        await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.Password, model.PasswordNew);
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
      UnitOfWorkManager uowManager = new UnitOfWorkManager();
      UserManager userManager = new UserManager(uowManager);
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
      UserManager userManager = new UserManager();
      User user = userManager.ChangeUserAlerts(User.Identity.GetUserId(), model.AlertsViaWebsite, model.AlertsViaEmail, model.WeeklyReviewViaEmail);
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Deactivate account from selected user.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/DeactivateAccount/{userId}")]
    public IHttpActionResult DeactivateAccount(string userId)
    {
      IUserManager userManager = new UserManager();
      userManager.DeactivateUserAccount(userId, true);
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Activate account from selected user.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/ActivateAccount/{userId}")]
    public IHttpActionResult ActivateAccount(string userId)
    {
      IUserManager userManager = new UserManager();
      userManager.DeactivateUserAccount(userId, false);
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Change role for selected user.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/ChangeRole/{userId}")]
    public IHttpActionResult ChangeRole(string userId, [FromBody]string roleName)
    {
      if (roleName.Equals("User") || roleName.Equals("Admin") || roleName.Equals("SuperAdmin"))
      {
        IUserManager userManager = new UserManager();
        string currentRole = userManager.GetRole(userId).Name;
        IdentityUserManager identityUserManager =
            HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
        identityUserManager.RemoveFromRole(userId, currentRole);
        identityUserManager.AddToRole(userId, roleName);
        return StatusCode(HttpStatusCode.NoContent);
      }    
      return StatusCode(HttpStatusCode.NotAcceptable);
    }
  }
}