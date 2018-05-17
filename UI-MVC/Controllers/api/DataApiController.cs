using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace BAR.UI.MVC.Controllers.api
{
	public class DataApiController : ApiController
	{
		private IDataManager dataManager;

		[HttpGet]
		[Route("api/Data/Synchronize")]
		[SubPlatformCheckAPI]
		public IHttpActionResult Synchronize()
		{
			dataManager = new DataManager();

			string content;
			if (dataManager.GetLastAudit() == null)
			{
				//content = "{}";

				//Test with fewer data 
				content = "{\"since\":\"2018-05-11 00:00\"}";
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

							//Syncronize recent data with all the widgets
							new WidgetManager().GenerateDataForMwidgets();
							//Update all items with recent data
							new ItemManager().FillItems();
							//Update weekly review alerts
							//Get the subplatformID from the SubPlatformCheckAPI attribute
							object _customObject = null;
							int suplatformID = -1;
							if (Request.Properties.TryGetValue("SubPlatformID", out _customObject))
							{
								suplatformID = (int)_customObject;
							}
							new UserManager().GenerateAlertsForWeeklyReview(suplatformID);


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
	}
}
