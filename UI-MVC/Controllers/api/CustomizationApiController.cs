using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAR.BL.Managers;

namespace BAR.UI.MVC.Controllers.api
{
    public class CustomizationApiController : ApiController
    {
		private ISubplatformManager platformManager;

		public IHttpActionResult Get()
		{
			platformManager = new SubplatformManager();
			return Ok();
		}
    }
}
