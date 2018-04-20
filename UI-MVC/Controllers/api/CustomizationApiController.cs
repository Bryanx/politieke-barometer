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
		[HttpGet]
		[Route("api/Customization/GetCustom/{platformId}")]
		public IHttpActionResult GetCustom(int platformId)
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
		[HttpPut]
		[Route("api/Customization/PutColor/{platformId}")]
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
		[HttpPut]
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
		[HttpPut]
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
		[HttpPut]
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
		[HttpPut]
		[Route("api/Customization/PutAddress/{platformId}")]
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

		/// <summary>
		/// Gives back all the questions
		/// </summary>
		[HttpGet]
		[Route("api/Customization/GetQuestions")]
		public IHttpActionResult GetQuestions()
		{
			platformManager = new SubplatformManager();
			IEnumerable<Question> requestedQuestions = platformManager.GetAllQuestions();

			if (requestedQuestions == null)
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(requestedQuestions);
		}

		/// <summary>
		/// Gives a specific question
		/// </summary>
		[HttpGet]
		[Route("api/Customization/GetQuestions/{questionId}")]
		public IHttpActionResult GetQuestion(int questionId)
		{
			platformManager = new SubplatformManager();
			Question requestedQuestion = platformManager.GetQuestion(questionId);

			if (requestedQuestion == null)
				return StatusCode(HttpStatusCode.NoContent);

			return Ok(requestedQuestion);
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
		[HttpPut]
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
		[HttpDelete]
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
