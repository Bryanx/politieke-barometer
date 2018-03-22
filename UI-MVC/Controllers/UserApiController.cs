using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BAR.BL.Controllers;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers {
    public class UserApiController : ApiController {
        ISubscriptionManager subManager = new SubscriptionManager();
        public static int Counter = 0;

        /// <summary>
        /// Get Request for alerts of a specific user (id)
        /// This request is used on the member
        /// </summary>
        [HttpGet]
        [Route("api/User/{id}/GetAlerts")]
        public IHttpActionResult GetAlerts(int id) {
            if (Counter == 0) {
                Counter++;
                SysController sys = new SysController();
                sys.DetermineTrending();
                sys.GenerateAlerts();
            }

            IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(id);
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
        [Route("api/User/{id}/Alert/{alertId}/Read")]
        public IHttpActionResult MarkAlertAsRead(int id, int alertId) {
            subManager.ChangeAlertToRead(id, alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}