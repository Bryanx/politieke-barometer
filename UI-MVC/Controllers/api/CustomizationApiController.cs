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
	/// <summary>
	/// This API-controller is used for the customization of the subplatforms.
	/// </summary>
    public class CustomizationApiController : ApiController
    {
		private ISubplatformManager platformManager;

		/// <summary>
		/// Gives back the customization of a specific subplatform
		/// </summary>
		public IHttpActionResult Get(string platformName)
		{
			platformManager = new SubplatformManager();
			Customization custom = platformManager.GetCustomization(platformName);

			if (custom == null)
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(custom);
		}

		/// <summary>
		/// Changes the webpage colors of a specific subplatform
		/// </summary>
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

		/// <summary>
		/// Changes the page alias of a specific subplatform
		/// </summary>
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

		/// <summary>
		/// Changes the privacy of a specific subplatform
		/// </summary>
		public IHttpActionResult PutPrivacy(string platformName, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePrivacyText(platformName, custom.PrivacyText, custom.PrivacyTitle);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the FAQ of a specific subplatform
		/// </summary>
		public IHttpActionResult PutFAQ(string platformName, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangeFAQTitle(platformName, custom.FAQTitle);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the address of a specific subplatform
		/// </summary>
		public IHttpActionResult PutAddress(string platformName, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangeAddress(platformName, custom.StreetAndHousenumber, custom.Zipcode, custom.City, custom.Country, custom.Email);

			return StatusCode(HttpStatusCode.NoContent);
		}
 	}
}
