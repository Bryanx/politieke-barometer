using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BAR.UI.MVC.Controllers.api
{
	public class DataApiController : ApiController
	{
		private IDataManager dataManager;
		private IWidgetManager widgetManager;
		private IItemManager itemManager;
    private ISubscriptionManager subscriptionManager;

		[HttpGet]
		[Route("api/Data/Synchronize")]
		[SubPlatformCheckAPI]
		public async Task<IHttpActionResult> SynchronizeAsync()
		{
			dataManager = new DataManager();
					
			string content;
			if (dataManager.GetLastAudit() == null)
			{
				content = "{}";

				//Test with fewer data 
				//content = "{\"since\":\"2018-04-15 00:00\"}";
			}
			else
			{
				string stringdate = dataManager.GetLastAudit().TimeStamp.ToString("yyyy-MM-dd HH:mm");
				content = String.Format("{{\"since\":\"{0}\"}}", stringdate);
			}
			int auditId = dataManager.AddAudit(DateTime.Now, false).SynchronizeAuditId;

			using (HttpClient client = new HttpClient())
			{
				//Make request
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, dataManager.GetDataSource(1).Url);
				request.Headers.Add("Accept", "application/json");
				request.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");

				request.Content = new StringContent(content, Encoding.UTF8, "application/json");

				//Send request
				HttpResponseMessage response = client.SendAsync(request).Result;

				//Read response
				if (response.IsSuccessStatusCode)
				{
					var json = response.Content.ReadAsStringAsync().Result;
					if (!dataManager.IsJsonEmpty(json))
					{
						var completed = dataManager.SynchronizeData(json);
						if (completed)
						{						
							dataManager.ChangeAudit(auditId);

							widgetManager = new WidgetManager();
							itemManager = new ItemManager();
              subscriptionManager = new SubscriptionManager();
              ControllerHelpers controllerHelpers = new ControllerHelpers();

							//Get the subplatformID from the SubPlatformCheckAPI attribute
							object _customObject = null;
							int suplatformID = -1;
							if (Request.Properties.TryGetValue("SubPlatformID", out _customObject))
							{
								suplatformID = (int)_customObject;
							}

							//Syncronize recent data with all the persons and themes
							widgetManager.GenerateDataForPersonsAndThemes();

							//Update all persons and themes with recent data
							itemManager.FillPersonesAndThemes();

							//Generate data for organisations
							widgetManager.GenerateDataForOrganisations();

							//Fill organisations with data
							itemManager.FillOrganisations();

							//Refesh items with old data
							itemManager.RefreshItemData(suplatformID);

							//Update weekly review alerts						
							new UserManager().GenerateAlertsForWeeklyReview(suplatformID);

              //Send weekly review notification to android
              await controllerHelpers.SendPushNotificationAsync("/topics/weeklyreview", "Weekly Review", "Er is een nieuwe weekly review beschikbaar.");

              //Send trending notifications to users
              foreach (SubAlert subAlert in subscriptionManager.GetUnsendedSubAlerts())
              {
                User user = subAlert.Subscription.SubscribedUser;
                await controllerHelpers.SendPushNotificationAsync(user.DeviceToken, "Trending", String.Format("%s is nu trending.", subAlert.Subscription.SubscribedItem.Name));
              }

							return StatusCode(HttpStatusCode.OK);
						}
						else
						{
							return StatusCode(HttpStatusCode.NotAcceptable);
						}
					}
					else
					{
            subscriptionManager = new SubscriptionManager();
            ControllerHelpers controllerHelpers = new ControllerHelpers();
            foreach (SubAlert subAlert in subscriptionManager.GetUnsendedSubAlerts())
            {
              User user = subAlert.Subscription.SubscribedUser;
              await controllerHelpers.SendPushNotificationAsync(user.DeviceToken, "Trending", String.Format("%s is nu trending.", subAlert.Subscription.SubscribedItem.Name));
            }
            return StatusCode(HttpStatusCode.NoContent);
          }
				}
				else
				{
					return StatusCode(HttpStatusCode.NotAcceptable);
				}
			}
		}
	}
}
