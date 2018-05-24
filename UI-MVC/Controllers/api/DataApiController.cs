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
		private IUserManager userManager;
		private ISubscriptionManager subscriptionManager;

		/// <summary>
		/// Sets the synchronize timer
		/// </summary>
		[HttpPost]
		[Route("api/Data/SetSynchronize/{interval}/{start}/{id}")]
		public IHttpActionResult SetSynchronize(int id, int interval, string start)
		{
			start = start.Substring(0, 2) + ":" + start.Substring(2, start.Length);
			dataManager = new DataManager();
			if (interval != 0 || start != "0")
			{
				dataManager.ChangeTimerInterval(id, interval);
				dataManager.ChangeStartTimer(id, start);
				return StatusCode(HttpStatusCode.Accepted);
			}
			else
			{
				return StatusCode(HttpStatusCode.NotAcceptable);
			}
		}

		/// <summary>
		/// Synchronizes with the database of textgain
		/// </summary>
		[HttpGet]
		[Route("api/Data/Synchronize/{id}")]
		[SubPlatformCheckAPI]
		public async Task<IHttpActionResult> Synchronize(int id)
		{
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}

			dataManager = new DataManager();
			itemManager = new ItemManager();

			List<Theme> themes = itemManager.GetAllThemesForSubplatform(suplatformID).ToList();

			//Making the string with all the themes. The source should validate
			string themeString = "\"themes\":{ ";
			for (int i = 0; i < themes.Count(); i++)
			{
				themeString += "\"";
				themeString += themes[i].Name;
				themeString += "\":[";
				List<Keyword> keywords = themes[i].Keywords.ToList();
				for (int j = 0; j < keywords.Count(); j++)
				{
					themeString += "\"";
					themeString += keywords[j].Name;
					themeString += "\"";
					if (j != keywords.Count() - 1)
					{
						themeString += ",";
					}
				}
				themeString += "]";
				if (i != themes.Count() - 1)
				{
					themeString += ",";
				}
			}
			themeString += "}";

			string content;
			if (dataManager.GetLastAudit() == null)
			{
				//content = "{}";

				//Test with fewer data 
				content = "{" +
				  "\"since\":\"2018-05-22 00:00\"," +
				  themeString +
				  "}";
			}
			else
			{
				string stringdate = dataManager.GetLastAudit().TimeStamp.ToString("yyyy-MM-dd HH:mm");
				content = "{" +
					"\"since\":\"" +
					stringdate +
					"\"," +
					themeString +
					"}";
			}
			int auditId = dataManager.AddAudit(DateTime.Now, false).SynchronizeAuditId;

			using (HttpClient client = new HttpClient())
			{
				//Make request
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, dataManager.GetDataSource(id).Url);
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
							userManager = new UserManager();
							ControllerHelpers controllerHelpers = new ControllerHelpers();

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
							if (new UserManager().GenerateAlertsForWeeklyReview(suplatformID))
							{
								//Send weekly review notification to android
								await controllerHelpers.SendPushNotificationAsync("/topics/weeklyreview", "Weekly Review", "Er is een nieuwe weekly review beschikbaar.");
							}

							//Send trending notifications to users
							foreach (SubAlert subAlert in subscriptionManager.GetUnsendedSubAlerts())
							{
								User user = subAlert.Subscription.SubscribedUser;
								if (user.DeviceToken != null)
								{
									await controllerHelpers.SendPushNotificationAsync(user.DeviceToken, "Trending", String.Format("%s is nu trending.", subAlert.Subscription.SubscribedItem.Name));
									subscriptionManager.ChangeSubAlertToSend(subAlert);
								}
								

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
						return StatusCode(HttpStatusCode.NoContent);
					}
				}
				else
				{
					return StatusCode(HttpStatusCode.NotAcceptable);
				}
			}
		}

		/// <summary>
		/// Sets an item to a deleted state
		/// </summary>
		[HttpPost]
		[Route("api/Data/DeleteItem/{dataSourceId}")]
		public IHttpActionResult ToggleDeleteItem(int dataSourceId)
		{
			dataManager = new DataManager();
			dataManager.RemoveDataSource(dataSourceId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Renames a datasource
		/// </summary>
		[HttpPost]
		[Route("api/Data/ChangeDataSource/{dataSourceId}/{interval}")]
		public IHttpActionResult RenameItem(int dataSourceId, int interval)
		{
			dataManager = new DataManager();
			dataManager.ChangeTimerInterval(dataSourceId, interval);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
