using System;
using System.Net;
using System.Web.Http;
using BAR.BL.Managers;

namespace BAR.UI.MVC.Controllers.api
{
  public class ItemApiController : ApiController
  {
    /// <summary>
    /// Delete item from logged-in user.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/DeleteItem/{itemId}")]
    public IHttpActionResult DeleteItem(string itemId)
    {
      IItemManager itemManager = new ItemManager();
      itemManager.ChangeItemActivity(Int32.Parse(itemId));
      return StatusCode(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Rename item from logged-in user.
    /// </summary>
    [HttpPost]
    [Route("api/Admin/RenameItem/{itemId}/{itemName}")]
    public IHttpActionResult RenameItem(string itemId, string itemName)
    {
      IItemManager itemManager = new ItemManager();
      itemManager.ChangeItemName(Int32.Parse(itemId), itemName);
      return StatusCode(HttpStatusCode.NoContent);
    }
  }
}