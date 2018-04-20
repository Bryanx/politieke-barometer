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

		public IHttpActionResult Get(string platformName)
		{
			platformManager = new SubplatformManager();
			Customization custom = platformManager.GetCustomization(platformName);

			if (custom == null)
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(custom);
		}

		public IHttpActionResult PutColor(string platformName, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePageColors
				(platformName, custom.PrimaryColor, custom.SecondairyColor, custom.TertiaryColor, custom.BackgroundColor, custom.TextColor);

			return StatusCode(HttpStatusCode.NoContent);
		}

		public IHttpActionResult PutAlias(string platformName, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePageText
				(platformName, custom.PersonAlias, custom.PersonsAlias, custom.OrganisationAlias, custom.OrganisationsAlias, custom.ThemeAlias, custom.ThemesAlias);

			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
