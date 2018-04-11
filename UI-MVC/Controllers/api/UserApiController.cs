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
    /// Toggles a Subscription for a specific user (based on wether the subscriptions exists or not).
    /// </summary>
    [HttpPost]
    [Route("api/ToggleSubscribe/{itemId}")]
    public IHttpActionResult ToggleSubscribe(int itemId)
    {
      ISubscriptionManager SubManager = new SubscriptionManager();
      string userId = User.Identity.GetUserId();
      SubManager.ToggleSubscription(userId, itemId);
      return StatusCode(HttpStatusCode.NoContent);
    }
    
    /// <summary>
    /// Updates a user account.
    /// </summary>
    [HttpPost]
    [Route("api/User/UpdateAccount")]
    public async Task<IHttpActionResult> UpdateAccount(SettingsViewModel model)
    {
      IdentityUserManager userManager =
          HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
      User user = await userManager.FindByIdAsync(User.Identity.GetUserId());
      if (await userManager.CheckPasswordAsync(user, model.Password))
      {
        await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.Password, model.PasswordNew);
        return StatusCode(HttpStatusCode.NoContent);
      }

      return StatusCode(HttpStatusCode.NotAcceptable);
    }
    
    /// <summary>
    /// Updates user profile.
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
    /// Updates user alert settings.
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
    /// Deactivate user account.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/DeactivateAccount/{userId}")]
    public IHttpActionResult DeactivateAccount(string userId)
    {
      IUserManager userManager = new UserManager();
      userManager.ChangeUserAccount(userId, true);
      return StatusCode(HttpStatusCode.NoContent);
    }
    
    /// <summary>
    /// Activate user account.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/ActivateAccount/{userId}")]
    public IHttpActionResult ActivateAccount(string userId)
    {
      IUserManager userManager = new UserManager();
      userManager.ChangeUserAccount(userId, false);
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