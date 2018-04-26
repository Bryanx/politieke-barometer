using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BAR.UI.MVC.Controllers.api
{
  public class SuperAdminApiController : ApiController
  {
    /// <summary>
    /// Add source.
    /// </summary>
    [HttpPost]
    [Route("api/SuperAdmin/AddSource")]
    public IHttpActionResult Addsource([FromBody]SourceManagement model)
    {
      IDataManager dataManager = new DataManager();
      var source = dataManager.AddSource(model.Name, model.Site);
      if (source != null)
      {
        return Ok(source.SourceId);
      }
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
        IDataManager dataManager = new DataManager();

        dataManager.RemoveSource(id);
        return StatusCode(HttpStatusCode.NoContent);
      }
        return StatusCode(HttpStatusCode.NotAcceptable);
    }
  }
}
