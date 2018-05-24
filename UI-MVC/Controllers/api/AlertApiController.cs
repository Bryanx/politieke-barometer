using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This controller is used for doing api-calls to work with alerts.
	/// </summary>
	public class AlertApiController : ApiController
	{
		private ISubscriptionManager subManager;

		/// <summary>
		/// Get Request for alerts of a specific user (id)
		/// This request is used on the member
		/// </summary>
		[HttpGet]
		[Route("api/User/GetAlerts")]
		public IHttpActionResult GetAlerts()
		{
			subManager = new SubscriptionManager();
			string userId = User.Identity.GetUserId();
			IEnumerable<UserAlert> userAlerts = subManager.GetUserAlerts(userId);
			IEnumerable<SubAlert> subAlerts = subManager.GetSubAlerts(userId);

			if (userAlerts == null || subAlerts == null || (userAlerts.Count() == 0 && subAlerts.Count() == 0)) return StatusCode(HttpStatusCode.NoContent);

			List<AlertDTO> lijst = new List<AlertDTO>();
			foreach (SubAlert alert in subAlerts)
			{
				AlertDTO alertDTO = new AlertDTO()
				{
					AlertId = alert.AlertId,
					Name = alert.Subscription.SubscribedItem.Name,
					TimeStamp = alert.TimeStamp,
					IsRead = alert.IsRead,
					ItemId = alert.Subscription.SubscribedItem.ItemId
				};
				lijst.Add(alertDTO);
			}
			foreach (UserAlert alert in userAlerts)
			{
				AlertDTO alertDTO = new AlertDTO()
				{
					AlertId = alert.AlertId,
					Name = alert.Subject,
					TimeStamp = alert.TimeStamp,
					IsRead = alert.IsRead,
				};
				lijst.Add(alertDTO);
			}

			return Ok(lijst.AsEnumerable());
		}

		/// <summary>
		/// Updates an alert from a specific user. Sets its property isRead to true.
		/// </summary>
		[HttpPut]
		[Route("api/User/Alert/{alertId}/Read")]
		public IHttpActionResult MarkAlertAsRead(int alertId)
		{
			subManager = new SubscriptionManager();
			subManager.ChangeAlertToRead(alertId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Removes an alert from a specific user.
		/// </summary>
		[HttpDelete]
		[Route("api/User/Alert/{alertId}/Delete")]
		public IHttpActionResult DeleteAlert(int alertId)
		{
			subManager = new SubscriptionManager();
			subManager.RemoveAlert(alertId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Gives the alerttype to the front-end
		/// </summary>
		[HttpGet]
		[Route("api/User/Alert/CheckAlert/{alertId}")]
		public string CheckAlertType(int alertId)
		{
			//Get alert
			subManager = new SubscriptionManager();
			Alert alert = subManager.GetAlert(alertId);
			if (alert == null) return null;

			//Retreive type
			if (alert is UserAlert) return "user alert";
			else return "sub alert";
		}
	}
}