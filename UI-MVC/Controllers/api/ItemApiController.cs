using System;
using System.Net;
using System.Web.Http;
using BAR.BL.Managers;

namespace BAR.UI.MVC.Controllers.api {
    public class ItemApiController : ApiController {
        
        private readonly IItemManager itemManager = new ItemManager();
        
        [HttpPost]
        [Route("api/Admin/DeleteItem/{itemId}")]
        public IHttpActionResult DeleteItem(string itemId)
        {
            itemManager.ChangeItemActivity(Int32.Parse(itemId));
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        [HttpPost]
        [Route("api/Admin/RenameItem/{itemId}/{itemName}")]
        public IHttpActionResult DeleteItem(string itemId, string itemName)
        {
            itemManager.ChangeItemName(Int32.Parse(itemId), itemName);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}