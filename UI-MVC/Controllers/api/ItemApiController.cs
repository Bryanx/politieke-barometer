using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;
using BAR.BL.Domain.Core;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This controller is used for doing api-calls to work with items.
	/// </summary>
	public class ItemApiController : ApiController
	{
		private IItemManager itemManager;
		private ISubplatformManager subplatformManager;

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
		/// Gets top 3 trending items
		/// </summary>
		[HttpGet]
		[Route("api/GetTopTrendingItems")]
		public IHttpActionResult GetTopTrendingItems()
		{
			itemManager = new ItemManager();
			List<Item> lijst = itemManager.GetAllItems()
				.OrderByDescending(m => m.TrendingPercentage)
				.Take(3)
				.ToList();
			return Ok(Mapper.Map(lijst, new List<ItemDTO>()));
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

		/// <summary>
		/// Creates a person item
		/// </summary>
		[HttpPost]
		[SubPlatformCheckAPI]
		[Route("api/Admin/CreatePerson")]
		public HttpResponseMessage CreatePerson()
		{
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			object _customObject = null;
			int subplatformID = -1;

			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject))
			{
				subplatformID = (int)_customObject;
			}

			itemManager = new ItemManager();

			Person p = (Person)itemManager.AddItem(ItemType.Person, "Maarten Jorens");

			itemManager.ChangePerson(p.ItemId, "google.com");
			itemManager.ChangeItemPlatform(p.ItemId, subplatformID);

			// Create a 201 response.
			var response = new HttpResponseMessage(HttpStatusCode.Created)
			{
				Content = new StringContent(p.ItemId.ToString())
			};

			response.Headers.Location =
			new Uri(Url.Link("Item", new { action = "Deatails", id = p.ItemId }));
			return response;

		}

		/// <summary>
		/// Creates a person item
		/// </summary>
		[HttpPost]
		[SubPlatformCheckAPI]
		[Route("api/Admin/CreateOrganisation")]
		public IHttpActionResult CreateOrganisation()
		{
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			object _customObject = null;
			int subplatformID = -1;

			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject))
			{
				subplatformID = (int)_customObject;
			}

			itemManager = new ItemManager();

			Organisation p = (Organisation)itemManager.AddItem(ItemType.Organisation, "De bosklappers");

			itemManager.ChangeOrganisation(p.ItemId, "www.google.com");
			itemManager.ChangeItemPlatform(p.ItemId, subplatformID);

			return StatusCode(HttpStatusCode.OK);
		}

		/// <summary>
		/// Creates a person item
		/// </summary>
		[HttpPost]
		[SubPlatformCheckAPI]
		[Route("api/Admin/CreateTheme")]
		public IHttpActionResult CreateTheme()
		{
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			object _customObject = null;
			int subplatformID = -1;

			if (Request.Properties.TryGetValue("SubPlatformID", out _customObject))
			{
				subplatformID = (int)_customObject;
			}

			itemManager = new ItemManager();

			Person p = (Person)itemManager.AddItem(ItemType.Person, "Maarten Jorens");

			itemManager.ChangePerson(p.ItemId, "google.com");
			itemManager.ChangeItemPlatform(p.ItemId, subplatformID);

			Person updatesPerson = itemManager.GetPersonWithDetails(p.ItemId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Retrieves more people from the same organisation.
		/// </summary>
		[HttpGet]
		[Route("api/GetMorePeopleFromOrg/{itemId}")]
		public IHttpActionResult GetMorePeopleFromOrg(string itemId)
		{
			itemManager = new ItemManager();
			int orgId = itemManager.GetPersonWithDetails(Int32.Parse(itemId)).Organisation.ItemId;
			List<Person> items = itemManager.GetAllPersons()
				.Where(p => p.Organisation.ItemId == orgId)
				.OrderByDescending(p => p.NumberOfMentions)
				.Take(6).ToList();
			return Ok(Mapper.Map(items, new List<ItemDTO>()));
		}
		
		/// <summary>
		/// Retrieves more people from the same organisation.
		/// </summary>
		[HttpGet]
		[Route("api/GetPeopleFromOrg/{itemId}")]
		public IHttpActionResult GetPeopleFromOrg(string itemId)
		{
			itemManager = new ItemManager();
			int orgId = Int32.Parse(itemId);
			List<Person> items = itemManager.GetAllPersons().Where(p => p.Organisation.ItemId == orgId).ToList();
			return Ok(Mapper.Map(items, new List<ItemDTO>()));
		}
	}
}