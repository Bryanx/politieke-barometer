using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Barometer.DAL;
using Barometer.Models;

namespace Barometer.Controllers {
    public class WidgetController : ApiController {
        
        WidgetManager mgr = new WidgetManager();
        
        public IHttpActionResult Get() {
            var responses = mgr.Read();
            if (responses == null || responses.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);
            return Ok(responses);
        }
        
        /**
         * Post is create!
         */
        public IHttpActionResult Post([FromBody] Widget widget) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (mgr.Exists(widget.Id)) return StatusCode(HttpStatusCode.Conflict);
            mgr.Insert(widget);
            //Return the URL on which the created object can be requested
            return CreatedAtRoute("DefaultApi"
                , new {controller = "Widget", id = widget.Id}
                , widget);
        }
        
        /**
         * Put is update!
         */
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

        /**
         * Put is update!
         */
        [Route("api/Widget/{id}/title")]
        public IHttpActionResult PutName(int id, [FromBody] string newTitle) {
            if (!mgr.Exists(id)) {
                return NotFound();
            }
            mgr.Update(id, newTitle);
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult Delete(int id) {
            if (!mgr.Exists(id)) {
                return NotFound();
            }
            mgr.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        
    }
}