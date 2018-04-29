﻿using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;

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
		/// Changes the name of an item for a given item id.
		/// </summary>
		[HttpGet]
		[Route("api/GetNumberOfMentions/{itemId}")]
		public IHttpActionResult GetNumberOfMentions(string itemId)
		{
			IDataManager dataManager = new DataManager();
			int numberOfMentions = dataManager.GetNumberInfo(Int32.Parse(itemId), DateTime.MinValue);
			return Ok(numberOfMentions);
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
	}
}