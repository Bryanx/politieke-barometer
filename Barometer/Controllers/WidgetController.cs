using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Barometer.Models;

namespace Barometer.Controllers {
    public class WidgetController : ApiController {
        
        public IHttpActionResult Get() {
            var responses = Widget.getWidgets();

            if (responses == null || responses.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);

            return Ok(responses);
        }
        
    }
}