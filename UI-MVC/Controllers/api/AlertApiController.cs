using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
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
			//TODO

			//subManager = new SubscriptionManager();
			//IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(User.Identity.GetUserId());
			//if (alertsToShow == null || alertsToShow.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

			////Made DTO class to prevent circular references
			//List<AlertDTO> lijst = new List<AlertDTO>();
			//foreach (Alert alert in alertsToShow)
			//{
			//	AlertDTO alertDto = new AlertDTO()
			//	{
			//		AlertId = alert.AlertId,
			//		Name = alert.Subscription.SubscribedItem.Name,
			//		TimeStamp = alert.TimeStamp,
			//		IsRead = alert.IsRead,
			//		itemId = alert.Subscription.SubscribedItem.ItemId

			//	};
			//	if (alert is UserAlert)
			//	{
			//		alertDto.Name = 
			//	}

			//}
			//return Ok(lijst.AsEnumerable());
			return Ok();
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
	}
}