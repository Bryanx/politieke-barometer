using System.Net;
using System.Web.Http;
using BAR.BL.Managers;
using BAR.BL.Domain.Core;
using BAR.UI.MVC.Models;
using BAR.UI.MVC.Attributes;
using System;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This API-controller is used for the customization of the subplatforms.
	/// </summary>
	///
	[SubPlatformCheckAPI]
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
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}

			return suplatformID;
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
		[Route("api/Customization/PutColor")]
		[SubPlatformCheckAPI]
		public IHttpActionResult PutColor([FromBody] CustomizationViewModel model)
		{
			platformManager = new SubplatformManager();

			object _customObject = null;
			int suplatformId = 1;
			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject)) suplatformId = (int)_customObject;

			if (model == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePageColors
				(suplatformId, model.PrimaryColor, model.PrimaryDarkerColor, model.PrimaryDarkestColor, model.SecondaryColor,
				model.SecondaryLighterColor, model.SecondaryDarkerColor, model.SecondaryDarkestColor, model.TertiaryColor, model.BackgroundColor, model.TextColor);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the page alias of a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PutAlias")]
		public IHttpActionResult PutAlias([FromBody] CustomizationViewModel model)
		{
			platformManager = new SubplatformManager();

			object _customObject = null;
			int suplatformId = 1;
			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject)) suplatformId = (int)_customObject;

			if (model == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePageText
				(suplatformId, model.PersonAlias, model.PersonsAlias, model.OrganisationAlias, model.OrganisationsAlias, model.ThemeAlias, model.ThemesAlias);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the privacy of a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PutPrivacy")]
		public IHttpActionResult PutPrivacy([FromBody] CustomizationViewModel custom)
		{
			platformManager = new SubplatformManager();

			object _customObject = null;
			int suplatformId = 1;
			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject)) suplatformId = (int)_customObject;

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangePrivacyText(suplatformId, custom.PrivacyText, custom.PrivacyTitle);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the FAQ of a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PutFAQ")]
		public IHttpActionResult PutFAQ([FromBody] CustomizationViewModel custom)
		{
			platformManager = new SubplatformManager();

			object _customObject = null;
			int suplatformId = 1;
			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject)) suplatformId = (int)_customObject;

			if (custom == null)
				return BadRequest("No question given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.AddQuestion(suplatformId, QuestionType.General, custom.FAQQuestion, custom.FAQAnswer);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the address of a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PutContact")]
		public IHttpActionResult PutContact([FromBody] CustomizationViewModel custom)
		{
			platformManager = new SubplatformManager();

			object _customObject = null;
			int suplatformId = 1;
			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject)) suplatformId = (int)_customObject;

			if (custom == null)
				return BadRequest("No customization given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.ChangeAddress(suplatformId, custom.StreetAndHousenumber, custom.Zipcode, custom.City, custom.Country, custom.Email);

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Creates a question for a specific subplatform
		/// </summary>
		[HttpPost]
		[Route("api/Customization/PostQuestion/")]
		public IHttpActionResult PostQuestion([FromBody] Question question)
		{
			platformManager = new SubplatformManager();

			object _customObject = null;
			int suplatformId = 1;
			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject)) suplatformId = (int)_customObject;

			if (question == null)
				return BadRequest("No question given");
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			platformManager.AddQuestion(suplatformId, QuestionType.General, question.Title, question.Answer);

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
		[Route("api/Subplatform/DeleteQuestion/{questionId}")]
		public IHttpActionResult DeleteQuestion(int questionId)
		{
			platformManager = new SubplatformManager();

			if (!platformManager.Exists(questionId))
				return NotFound();

			try
			{
				platformManager.RemoveQuestion(questionId);
#pragma warning disable CS0168 // Variable is declared but never used
			}
			catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				//Do Nothing
			}			
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}
