using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using BAR.UI.MVC.DAL;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers {
    
    /// This class is used to transfer all widget information from UI to the managers.
    /// The api calls are in ajax requests on the dashboard index page.
    public class WidgetController : ApiController {
        
        WidgetManager mgr = new WidgetManager();
        
        ///Reads all widgets and returns them (possibly in json)
        public IHttpActionResult Get() {
            var responses = mgr.Read();
            if (responses == null || responses.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);
            return Ok(responses);
        }
        
        /// <summary>
        /// Creates a new Widget from the body of the post request.
        /// If the widget already exists, returns 409 Conflict
        /// </summary>
        /// <param name="widget"></param>
        /// <returns>The URL on which the created object can be requested</returns>
        public IHttpActionResult Post([FromBody] Widget widget) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (mgr.Exists(widget.Id)) return StatusCode(HttpStatusCode.Conflict);
            mgr.Insert(widget);
            return CreatedAtRoute("DefaultApi"
                , new {controller = "Widget", id = widget.Id}
                , widget);
        }
        
        /// <summary>
        /// Updates an existing widget.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="widget"></param>
        /// <returns>If all goes well, 204 No Content is returned</returns>
        public IHttpActionResult Put(int id, [FromBody] Widget widget) {
            if (widget == null)
                return BadRequest("No widget given");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != widget.Id)
                return BadRequest("Id doesn't match");
            if (!mgr.Exists(widget.Id))
                return NotFound();
            mgr.Update(widget);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// Temp test update to give a widget a new title
        [Route("api/Widget/{id}/title")]
        public IHttpActionResult PutName(int id, [FromBody] string newTitle) {
            if (!mgr.Exists(id)) {
                return NotFound();
            }
            mgr.Update(id, newTitle);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// Temp delete a widget.
        public IHttpActionResult Delete(int id) {
            if (!mgr.Exists(id)) {
                return NotFound();
            }
            mgr.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
    }
}