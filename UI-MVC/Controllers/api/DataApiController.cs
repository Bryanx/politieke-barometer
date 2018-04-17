using BAR.BL.Managers;
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
		private DataManager dataManager;

		[HttpGet]
		[Route("api/Data/Synchronize")]
		public IHttpActionResult Synchronize()
		{
			dataManager = new DataManager();

			//Get Timestamp
			string content;
			if (dataManager.GetLastAudit() == null) content = "{}";		
			else
			{
				content = String.Format("{\"since\":\"{0}\"}", dataManager.GetLastAudit().TimeStamp.ToString("yyyy-MM-dd hh:mm"));
			}

			using (HttpClient client = new HttpClient())
			{
				//Make request
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://kdg.textgain.com/query");
				request.Headers.Add("Accept", "application/json");
				request.Headers.Add("X-API-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");
				request.Content = new StringContent(content, Encoding.UTF8, "application/json");

				//Send request
				HttpResponseMessage response = client.SendAsync(request).Result;

				//Read response
				if (response.IsSuccessStatusCode)
				{
					var json = response.Content.ReadAsStringAsync().Result;
					var success = dataManager.SynchronizeData(json);
					dataManager.AddAudit(DateTime.Now, success);
					return StatusCode(HttpStatusCode.OK);
				}
				else return StatusCode(HttpStatusCode.NotAcceptable);	
			}
		}
	}
}
