using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAR.BL.Managers;
using BAR.BL.Domain.Core;

namespace BAR.UI.MVC.Controllers.api
{
    public class CustomizationApiController : ApiController
    {
		private ISubplatformManager platformManager;

		public IHttpActionResult Get()
		{
			platformManager = new SubplatformManager();
			Customization custom = platformManager.GetCustomization("politiek");

			if (custom == null) return StatusCode(HttpStatusCode.NoContent);
			return Ok(custom);
		}
    }
}
