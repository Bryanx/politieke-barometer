using System;
using System.Net;
using System.Web.Http;
using BAR.BL.Managers;

namespace BAR.UI.MVC.Controllers.api {
    public class ItemApiController : ApiController {
        
        private readonly IItemManager itemManager = new ItemManager();
        
        /// <summary>
        /// Deleted status of an item is toggled.
        /// </summary>
        [HttpPost]
        [Route("api/Admin/ToggleDeleteItem/{itemId}")]
        public IHttpActionResult ToggleDeleteItem(string itemId)
        {
            itemManager.ChangeItemActivity(Int32.Parse(itemId));
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        /// <summary>
        /// Changes the name of an item for a given item id.
        /// </summary>
        [HttpPost]
        [Route("api/Admin/RenameItem/{itemId}/{itemName}")]
        public IHttpActionResult RenameItem(string itemId, string itemName)
        {
            itemManager.ChangeItemName(Int32.Parse(itemId), itemName);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}