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
			IEnumerable<Alert> alertsToShow = subManager.GetAllAlerts(User.Identity.GetUserId());
			if (alertsToShow == null || !alertsToShow.Any()) return StatusCode(HttpStatusCode.NoContent);

			//Made DTO class to prevent circular references
			List<AlertDTO> lijst = new List<AlertDTO>();
			foreach (Alert alert in alertsToShow)
			{
				lijst.Add(new AlertDTO()
				{
					AlertId = alert.AlertId,
					Name = alert.Subscription.SubscribedItem.Name,
					TimeStamp = alert.TimeStamp,
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
		public IHttpActionResult MarkAlertAsRead(int alertId)
		{
			subManager = new SubscriptionManager();
			subManager.ChangeAlertToRead(User.Identity.GetUserId(), alertId);
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
			subManager.RemoveAlert(User.Identity.GetUserId(), alertId);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}