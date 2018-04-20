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
		public IHttpActionResult Get(int platformId)
		{
			platformManager = new SubplatformManager();
			Customization custom = platformManager.GetCustomization(platformId);

			if (custom == null)
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(custom);
		}

		/// <summary>
		/// Changes the webpage colors of a specific subplatform
		/// </summary>
		public IHttpActionResult PutColor(int platformId, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePageColors
				(platformId, custom.PrimaryColor, custom.SecondairyColor, custom.TertiaryColor, custom.BackgroundColor, custom.TextColor);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the page alias of a specific subplatform
		/// </summary>
		public IHttpActionResult PutAlias(int platformId, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePageText
				(platformId, custom.PersonAlias, custom.PersonsAlias, custom.OrganisationAlias, custom.OrganisationsAlias, custom.ThemeAlias, custom.ThemesAlias);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the privacy of a specific subplatform
		/// </summary>
		public IHttpActionResult PutPrivacy(int platformId, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePrivacyText(platformId, custom.PrivacyText, custom.PrivacyTitle);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the FAQ of a specific subplatform
		/// </summary>
		public IHttpActionResult PutFAQ(int platformId, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangeFAQTitle(platformId, custom.FAQTitle);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the address of a specific subplatform
		/// </summary>
		public IHttpActionResult PutAddress(int platformId, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangeAddress(platformId, custom.StreetAndHousenumber, custom.Zipcode, custom.City, custom.Country, custom.Email);

			return StatusCode(HttpStatusCode.NoContent);
		}
 	}
}
