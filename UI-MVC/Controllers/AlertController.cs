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
        /// Get Request geeft alerts de user met parameter id.
        /// Wordt gebruikt in een ajax call vanaf pagina's van de betreffende user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int id) {
            SysController sys = new SysController();
            sys.DetermineTrending();
            sys.GenerateAlerts();

            IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(id);
            if (alertsToShow == null || alertsToShow.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
            
            //DTO aangemaakt om circulaire referentie te voorkomen
            List<AlertDTO> lijst = new List<AlertDTO>();
            foreach (Alert alert in alertsToShow)
            {
                lijst.Add(new AlertDTO() {
                    //Momenteel even ingesteld dat je alerts kan krijgen van slechts 1 item, dit om 1miljoen alerts te voorkomen
                    AlertId=alert.Subscription.SubscribedItem.ItemId,
                    Name=alert.Subscription.SubscribedItem.Name,
                    TimeStamp=alert.TimeStamp
                });
            }
            return Ok(lijst.AsEnumerable());
        }
    }
}