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
    /// Removes a Subscription from a specific user.
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

    [HttpPost]
    [Route("api/User/UpdateAlerts")]
    public IHttpActionResult UpdateAlerts(SettingsViewModel model)
    {
      UserManager userManager = new UserManager();
      User user = userManager.ChangeUserAlerts(User.Identity.GetUserId(), model.AlertsViaWebsite, model.AlertsViaEmail, model.WeeklyReviewViaEmail);
      return StatusCode(HttpStatusCode.NoContent);
    }

    [HttpPost]
    [Route("api/Admin/DeactivateAccount/{userId}")]
    public IHttpActionResult DeactivateAccount(string userId)
    {
      IUserManager userManager = new UserManager();
      userManager.ChangeUserAccount(userId, true);
      return StatusCode(HttpStatusCode.NoContent);
    }

    [HttpPost]
    [Route("api/Admin/ActivateAccount/{userId}")]
    public IHttpActionResult ActivateAccount(string userId)
    {
      IUserManager userManager = new UserManager();
      userManager.ChangeUserAccount(userId, false);
      return StatusCode(HttpStatusCode.NoContent);
    }
  }
}