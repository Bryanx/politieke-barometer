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
        public ISubscriptionManager SubManager = new SubscriptionManager();
        public IUserManager UserManager = new UserManager();
        public static bool FirstCall = true;

        /// <summary>
        /// Get Request for alerts of a specific user (id)
        /// This request is used on the member
        /// </summary>
        [HttpGet]
        [Route("api/User/{id}/GetAlerts")]
        public IHttpActionResult GetAlerts(int id) {
            //TODO: Remove counter, temporary solution because db is rebuild on every load.
            if (FirstCall) {
                FirstCall = false;
                SysController sys = new SysController();
                sys.DetermineTrending();
                sys.GenerateAlerts();
            }

            IEnumerable<Alert> alertsToShow = SubManager.GetAllAlerts(id);
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
        /// Gets a user.
        /// </summary>
        [HttpGet]
        [Route("api/User/{id}")]
        public IHttpActionResult GetUser(int id) {
            User user = UserManager.GetUser(id);
            return Ok(user);
        }
        
        /// <summary>
        /// Updates/Creates a user.
        /// </summary>
        [HttpPost]
        [Route("api/User/{id}")]
        public IHttpActionResult UpdateUser(UserSubscribedPeopleDTO usr) {
            User user = UserManager.GetUser(usr.User.UserId);
            User newUser = user.CopyTo<User>(usr.User);
            UserManager.ChangeUser(newUser);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        
        /// <summary>
        /// Updates an alert from a specific user. Sets its property isRead to true.
        /// </summary>
        [HttpPut]
        [Route("api/User/{id}/Alert/{alertId}/Read")]
        public IHttpActionResult MarkAlertAsRead(int id, int alertId) {
            SubManager.ChangeAlertToRead(id, alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Removes an alert from a specific user.
        /// </summary>
        [HttpDelete]
        [Route("api/User/{userId}/Alert/{alertId}/Delete")]
        public IHttpActionResult DeleteAlert(int userId, int alertId) {
            SubManager.RemoveAlert(userId, alertId);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Removes a Subscription from a specific user.
        /// </summary>
        [HttpDelete]
        [Route("api/User/{id}/Subscription/{subId}/Delete")]
        public IHttpActionResult DeleteSubscription(int subId) {
            SubManager.RemoveSubscription(subId);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}