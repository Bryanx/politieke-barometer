using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using BAR.BL.Controllers;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;

namespace BAR.UI.MVC.Controllers.api {
    public class AlertApiController : ApiController {
        
        public static bool FirstCall = true;

        /// <summary>
        /// Get Request for alerts of a specific user (id)
        /// This request is used on the member
        /// </summary>
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("api/User/GetAlerts")]
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
        [System.Web.Mvc.HttpPut]
        [System.Web.Mvc.Route("api/User/Alert/{alertId}/Read")]
        public IHttpActionResult MarkAlertAsRead(int alertId) {
            ISubscriptionManager SubManager = new SubscriptionManager();
            SubManager.ChangeAlertToRead(User.Identity.GetUserId(), alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Removes an alert from a specific user.
        /// </summary>
        [System.Web.Mvc.HttpDelete]
        [System.Web.Mvc.Route("api/User/Alert/{alertId}/Delete")]
        public IHttpActionResult DeleteAlert(int alertId) {
            ISubscriptionManager SubManager = new SubscriptionManager();
            SubManager.RemoveAlert(User.Identity.GetUserId(), alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}