﻿using System;
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

namespace BAR.UI.MVC.Controllers.api {
    public class UserApiController : ApiController {
        public static bool FirstCall = true;

        /// <summary>
        /// Get Request for alerts of a specific user (id)
        /// This request is used on the member
        /// </summary>
        [HttpGet]
        [Route("api/User/GetAlerts")]
        public IHttpActionResult GetAlerts() {
            ISubscriptionManager SubManager = new SubscriptionManager();
            //TODO: Remove counter, temporary solution because db is rebuild on every load.
            if (FirstCall) {
                FirstCall = false;
                SysController sys = new SysController();
                sys.DetermineTrending();
                sys.GenerateAlerts();
            }

            IEnumerable<Alert> alertsToShow = SubManager.GetAllAlerts(User.Identity.GetUserId());
            if (alertsToShow == null || alertsToShow.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            
            //Made DTO class to prevent circular references
            List<AlertDTO> lijst = new List<AlertDTO>();
            foreach (Alert alert in alertsToShow) {
                var date = (DateTime) alert.TimeStamp;
                lijst.Add(new AlertDTO() {
                    AlertId=alert.AlertId,
                    Name=alert.Subscription.SubscribedItem.Name,
                    TimeStamp=date,
                    IsRead = alert.IsRead
                });
            }
            return Ok(lijst.AsEnumerable());
        }

        /// <summary>
        /// Updates an alert from a specific user. Sets its property isRead to true.
        /// </summary>
        [HttpPut]
        [Route("api/User/Alert/{alertId}/Read")]
        public IHttpActionResult MarkAlertAsRead(int alertId) {
            ISubscriptionManager SubManager = new SubscriptionManager();
            SubManager.ChangeAlertToRead(User.Identity.GetUserId(), alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Removes an alert from a specific user.
        /// </summary>
        [HttpDelete]
        [Route("api/User/Alert/{alertId}/Delete")]
        public IHttpActionResult DeleteAlert(int alertId) {
            ISubscriptionManager SubManager = new SubscriptionManager();
            SubManager.RemoveAlert(User.Identity.GetUserId(), alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("api/Subscribe/{itemId}")]
        public IHttpActionResult CreateSubscription(int itemId) {
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
        public IHttpActionResult DeleteSubscription(int itemId) {
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
      IdentityUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<IdentityUserManager>();
      User user = await userManager.FindByIdAsync(User.Identity.GetUserId());
      if (await userManager.CheckPasswordAsync(user, model.Password))
      {
        await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.Password, model.PasswordNew);
        return StatusCode(HttpStatusCode.NoContent);
      }
      return StatusCode(HttpStatusCode.NotAcceptable);
    }
  }
}