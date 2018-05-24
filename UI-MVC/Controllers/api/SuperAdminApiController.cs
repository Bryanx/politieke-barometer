using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using System.Net;
using System.Web.Http;
using BAR.BL.Domain.Data;

namespace BAR.UI.MVC.Controllers.api
{
	public class SuperAdminApiController : ApiController
	{
		private IDataManager dataManager;

		/// <summary>
		/// Add source.
		/// </summary>
		[HttpPost]
		[Route("api/SuperAdmin/AddSource")]
		public IHttpActionResult Addsource([FromBody]GeneralManagementViewModel model)
		{
			dataManager = new DataManager();
			Source source = dataManager.AddSource(model.Name, model.Site);

			if (source != null)
				return Ok(source.SourceId);

			return StatusCode(HttpStatusCode.NotAcceptable);
		}

		/// <summary>
		/// Remove source.
		/// </summary>
		[HttpPost]
		[Route("api/SuperAdmin/RemoveSource")]
		public IHttpActionResult RemoveSource([FromBody]string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				dataManager = new DataManager();
				dataManager.RemoveSource(id);
				return StatusCode(HttpStatusCode.NoContent);
			}
			return StatusCode(HttpStatusCode.NotAcceptable);
		}

		/// <summary>
		/// Add source.
		/// </summary>
		[HttpPost]
		[Route("api/SuperAdmin/AddSubplatform")]
		public IHttpActionResult AddSubplatform([FromBody]SubPlatformManagement model)
		{
			ISubplatformManager subplatformManager = new SubplatformManager();
			var subplatform = subplatformManager.AddSubplatform(model.Name);
			if (subplatform != null)
			{
				return Ok(subplatform.SubPlatformId);
			}
			return StatusCode(HttpStatusCode.NotAcceptable);
		}
	}
}
