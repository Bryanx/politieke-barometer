using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This controller is used for doing api-calls to work with items.
	/// </summary>
	public class ItemApiController : ApiController
	{
		private IItemManager itemManager;
		
		/// <summary>
		/// Returns all items for search suggestions.
		/// </summary>
		[HttpGet]
    [SubPlatformCheckAPI]
		[Route("api/GetSearchItems")]
		public IHttpActionResult GetSearchItems()
		{
      //Get the subplatformID from the SubPlatformCheckAPI attribute
      object _customObject = null;
      int suplatformID = -1;

      if (Request.Properties.TryGetValue("SubPlatformID", out _customObject))
      {
        suplatformID = (int)_customObject;
      }


      itemManager = new ItemManager();
			var lijst = itemManager.GetAllItems()
        .Where(item => item.SubPlatform.SubPlatformId == suplatformID)
        .Select(i => new {value=i.Name, data=i.ItemId});
			return Ok(lijst);
		}

		/// <summary>
		/// Deleted status of an item is toggled.
		/// </summary>
		[HttpPost]
		[Route("api/Admin/ToggleDeleteItem/{itemId}")]
		public IHttpActionResult ToggleDeleteItem(string itemId)
		{
			itemManager = new ItemManager();
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
			itemManager = new ItemManager();
			itemManager.ChangeItemName(Int32.Parse(itemId), itemName);
			return StatusCode(HttpStatusCode.NoContent);
		}
		
		/// <summary>
		/// Retrieves an item.
		/// </summary>
		[HttpGet]
		[Route("api/GetItemWithDetails/{itemId}")]
		public IHttpActionResult GetItemWithDetails(string itemId)
		{
			itemManager = new ItemManager();
			Item item = itemManager.GetPersonWithDetails(Int32.Parse(itemId));
			return Ok(item);
		}
		
		/// <summary>
		/// Updates an item
		/// </summary>
		[HttpPost]
		[Route("api/Admin/UpdateItem/{itemId}")]
		public IHttpActionResult UpdateItem(string itemId, [FromBody] ItemViewModels.PersonViewModel model)
		{
			itemManager = new ItemManager();
			itemManager.ChangePerson(Int32.Parse(itemId), model.DateOfBirth, model.Gender, model.Position, model.District);						
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}