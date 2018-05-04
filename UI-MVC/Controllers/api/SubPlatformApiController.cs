using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAR.BL.Managers;
using BAR.BL.Domain.Core;
using BAR.UI.MVC.Models;
using AutoMapper;
using BAR.UI.MVC.App_GlobalResources;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This API-controller is used for the customization of the subplatforms.
	/// </summary>
	public class SubPlatformApiController : ApiController
	{
		private ISubplatformManager platformManager;
		private IUserManager userManager;

		/// <summary>
		/// Gives back the id of the subdomain related to 
		/// </summary>
		[HttpGet]
		[Route("api/Customization/GetPlatformId/{platformName}")]
		public int GetPlatformId(string platformName)
		{
			platformManager = new SubplatformManager();
			SubPlatform platform = platformManager.GetSubPlatform(platformName);

			if (platform == null) return -1;
			else return platform.SubPlatformId;
		}

		/// <summary>
		/// Gives back the customization of a specific subplatform
		/// </summary>
		[HttpGet]
		[Route("api/Customization/GetCustomForPlatform/{platformId}")]
		public IHttpActionResult GetCustomForPlatform(int platformId)
		{
			userManager = new UserManager();
			Customization custom = new SubplatformManager().GetCustomization(platformId);

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(custom);
		}

		/// <summary>
		/// Changes the webpage colors of a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PutColor/{platformId}")]
		public IHttpActionResult PutColor(int platformId, [FromBody] CustomizationViewModel custom)
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
		[HttpPost]
		[Route("api/Customization/PutAlias/{platformId}")]
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
		[HttpPost]
		[Route("api/Customization/PutPrivacy/{platformId}")]
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
		[HttpPost]
		[Route("api/Customization/PutFAQ/{platformId}")]
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
		[HttpPost]
		[Route("api/Customization/PutContact/{platformId}")]
		public IHttpActionResult PutContact(int platformId, [FromBody] Customization custom)
		{
			platformManager = new SubplatformManager();

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangeAddress(platformId, custom.StreetAndHousenumber, custom.Zipcode, custom.City, custom.Country, custom.Email);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Creates a question for a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PostQuestion/{platformId}")]
		public IHttpActionResult PostQuestion(int platformId, [FromBody] Question question)
		{
			platformManager = new SubplatformManager();

			if (question == null)
				return BadRequest("No question given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.AddQuestion(platformId, question.QuestionType, question.Title, question.Answer);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes a specific question
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PutQuestion/{questionId}")]
		public IHttpActionResult PutQuestion(int questionId, [FromBody] Question question)
		{
			platformManager = new SubplatformManager();

			if (question == null)
				return BadRequest("No question given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (questionId == question.QuestionId)
				return BadRequest("Id doesn't match");

			platformManager.ChangeQuestion(questionId, question.QuestionType, question.Title, question.Answer);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Deletes a question
		/// </summary>
		[HttpPost]
		[Route("api/Customization/DeleteQuestion/{platformId}")]
		public IHttpActionResult DeleteQuestion(int questionId)
		{
			platformManager = new SubplatformManager();
			if (!platformManager.Exists(questionId))
				return NotFound();

			platformManager.RemoveQuestion(questionId);

			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
