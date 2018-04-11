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

namespace BAR.UI.MVC.Controllers.api {
    public class AlertApiController : ApiController {
        
        private ISubscriptionManager SubManager = new SubscriptionManager();
        private static bool firstRun = true;
        
        /// <summary>
        /// Get Request for alerts of a specific user (id)
        /// This request is used on the member
        /// </summary>
        [HttpGet]
        [Route("api/User/GetAlerts")]
        public IHttpActionResult GetAlerts() {
            //TODO: Remove counter, temporary solution because db is rebuild on every load.
            if (firstRun) {
                firstRun = false;
                SysController sys = new SysController();
                sys.DetermineTrending();
                sys.GenerateAlerts();
            }

            IEnumerable<Alert> alertsToShow = SubManager.GetAllAlerts(User.Identity.GetUserId());
            if (alertsToShow == null || !alertsToShow.Any()) return StatusCode(HttpStatusCode.NoContent);
            
            //Made DTO class to prevent circular references
            List<AlertDTO> lijst = new List<AlertDTO>();
            foreach (Alert alert in alertsToShow) {
                lijst.Add(new AlertDTO() {
                    AlertId=alert.AlertId,
                    Name=alert.Subscription.SubscribedItem.Name,
                    TimeStamp=alert.TimeStamp,
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
            SubManager.ChangeAlertToRead(User.Identity.GetUserId(), alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Removes an alert from a specific user.
        /// </summary>
        [HttpDelete]
        [Route("api/User/Alert/{alertId}/Delete")]
        public IHttpActionResult DeleteAlert(int alertId) {
            SubManager.RemoveAlert(User.Identity.GetUserId(), alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}