using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BAR.BL.Controllers;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers {
    public class AlertController : ApiController {
        
        ISubscriptionManager subManager = new SubscriptionManager();

        /// <summary>
        /// Get Request for alerts of a specific user (id)
        /// This request is used on the member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int id) {
            SysController sys = new SysController();
            sys.DetermineTrending();
            sys.GenerateAlerts();

            IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(id);
            if (alertsToShow == null || alertsToShow.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            
            //Made DTO class to prevent circular references
            List<AlertDTO> lijst = new List<AlertDTO>();
            foreach (Alert alert in alertsToShow)
            {
                lijst.Add(new AlertDTO() {
                    //TODO: Currently only 1 item can produce 1 alert, this can be changed later on.
                    AlertId=alert.Subscription.SubscribedItem.ItemId,
                    Name=alert.Subscription.SubscribedItem.Name,
                    TimeStamp=alert.TimeStamp
                });
            }
            return Ok(lijst.AsEnumerable());
        }
    }
}