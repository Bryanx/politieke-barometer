using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Attributes;
using BAR.UI.MVC.Models;
using WebGrease.Css.Extensions;
using BAR.BL.Domain.Widgets;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This controller is used for doing api-calls to work with items.
	/// </summary>
	[SubPlatformCheckAPI]
	public class ItemApiController : ApiController
	{
		private IItemManager itemManager;

		/// <summary>
		/// Returns all items for search suggestions.
		/// </summary>
		[HttpGet]
		[Route("api/GetSearchItems")]
		public IHttpActionResult GetSearchItems()
		{
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}

			itemManager = new ItemManager();
			var lijst = itemManager.GetAllItems()
				.Where(item => item.SubPlatform.SubPlatformId == suplatformID)
				.Select(item => new { value = item.Name, data = item.ItemId }).AsEnumerable();

			return Ok(lijst);
		}

		/// <summary>
		/// Gets top 3 trending items with details for specific type 
		/// </summary>
		[HttpGet]
		[Route("api/GetTopTrendingItems/{itemType}")]
		public IHttpActionResult GetTopTrendingItems(int itemType)
		{
			//Get the subplatformID from the SubPlatformCheckAPI attribute
			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}

			ItemType type = (ItemType)itemType;
			itemManager = new ItemManager();
			List<Item> lijst = new List<Item>();

			if (type == ItemType.Person)
			{
				itemManager.GetMostTrendingItemsForType(suplatformID, ItemType.Person, 3)
					.ForEach(item => lijst.Add(itemManager.GetPersonWithDetails(item.ItemId)));
			}
			else if (type == ItemType.Organisation)
			{
				itemManager.GetMostTrendingItemsForType(suplatformID, ItemType.Organisation, 3)
					.ForEach(item => lijst.Add(itemManager.GetOrganisationWithDetails(item.ItemId)));
			}
			else
			{
				itemManager.GetMostTrendingItemsForType(suplatformID, ItemType.Theme, 3)
					.ForEach(item => lijst.Add(itemManager.GetThemeWithDetails(item.ItemId)));
			}

			return Ok(Mapper.Map(lijst, new List<Item>()));
		}

		/// <summary>
		/// Deleted status of an item is toggled.
		/// </summary>
		[HttpPost]
		[Route("api/Admin/ToggleDeleteItem/{itemId}")]
		public IHttpActionResult ToggleDeleteItem(int itemId)
		{
			itemManager = new ItemManager();
			itemManager.ChangeItemActivity(itemId);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Changes the name of an item for a given item id.
		/// </summary>
		[HttpPost]
		[Route("api/Admin/RenameItem/{itemId}/{itemName}")]
		public IHttpActionResult RenameItem(int itemId, string itemName)
		{
			if (itemName == null) return BadRequest("Uncorrect parameters");
			itemManager = new ItemManager();
			itemManager.ChangeItemName(itemId, itemName);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Retrieves an item.
		/// </summary>
		[HttpGet]
		[Route("api/GetItemWithDetails/{itemId}")]
		public IHttpActionResult GetItemWithDetails(int itemId)
		{
			itemManager = new ItemManager();
			Item item = itemManager.GetPersonWithDetails(itemId);
			return Ok(item);
		}

		/// <summary>
		/// Updates an item
		/// </summary>
		[HttpPost]
		[Route("api/Admin/UpdateItem/{itemId}")]
		public IHttpActionResult UpdateItem(int itemId, [FromBody] ItemViewModels.PersonViewModel model)
		{
			itemManager = new ItemManager();
			itemManager.ChangePerson(itemId, model.DateOfBirth, model.Gender, model.Position, model.District);
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Retrieves more people from the same organisation.
		/// </summary>
		[HttpGet]
		[Route("api/GetMorePeopleFromOrg/{itemId}")]
		public IHttpActionResult GetMorePeopleFromOrg(int itemId)
		{
			itemManager = new ItemManager();
			Person requestedPerson = itemManager.GetPersonWithDetails(itemId);

			if (requestedPerson == null)
			{
				return Ok(Mapper.Map(new List<Person>(), new List<ItemDTO>()));
			}
			else
			{
				int orgId = requestedPerson.Organisation.ItemId;
				List<Person> items = itemManager.GetAllPersons()
												.Where(person => person.Organisation.ItemId == orgId)
												.OrderByDescending(person => person.NumberOfMentions)
												.Take(6)
												.ToList();
				return Ok(Mapper.Map(items, new List<ItemDTO>()));
			}
		}

		/// <summary>
		/// Retrieves more people from the same organisation.
		/// </summary>
		[HttpGet]
		[Route("api/GetPeopleFromOrg/{itemId}")]
		public IHttpActionResult GetPeopleFromOrg(int itemId)
		{
			itemManager = new ItemManager();
			try
			{
				List<Person> items = itemManager.GetAllPersons().Where(p => p.Organisation.ItemId == itemId).ToList();
				return Ok(Mapper.Map(items, new List<ItemDTO>()));
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				return Ok(Mapper.Map(new List<Person>(), new List<ItemDTO>()));
			}

		}

		/// <summary>
		/// Determines the type of an item
		/// needed to determine the url.
		/// </summary>
		[HttpGet]
		[Route("api/checkItemType/{itemId}")]
		public string GetItemType(int itemId)
		{
			if (itemId < 0) return null;

			//Get item
			itemManager = new ItemManager();
			Item item = itemManager.GetItem(itemId);

			//return correct type
			if (item is Person) return "Person";
			else if (item is Organisation) return "Organisation";
			else if (item is Theme) return "Theme";
			else return null;
		}

		/// <summary>
		/// Get stories from person
		/// </summary>
		[HttpGet]
		[Route("api/GetStories/{itemId}")]
		public IHttpActionResult GetStories(int itemId)
		{
			IDataManager dataManager = new DataManager();
			List<string> urls = dataManager.GetUrlsForItem(itemId).ToList();
			return Ok(urls.Distinct().Take(9));
		}

		/// <summary>
		/// Get geodata for inside a map
		/// </summary>
		[HttpGet]
		[Route("api/GetGeoData")]
		public IHttpActionResult GetGeoData()
		{
			IWidgetManager widgetManager = new WidgetManager();
			Widget geoWidget = widgetManager.GetGeoLocationWidget();
			if (geoWidget == null) return NotFound();

			List<WidgetDataDTO> widgetDto = Mapper.Map(geoWidget?.WidgetDatas, new List<WidgetDataDTO>());
			return Ok(widgetDto.FirstOrDefault());
		}

		/// <summary>
		/// Retrieves the top 3 persons per district
		/// </summary>
		[HttpGet]
		[Route("api/GetPopularPersonsPerDistrict")]
		[SubPlatformCheckAPI]
		public IHttpActionResult GetPopularPersonsPerDistrict()
		{
			itemManager = new ItemManager();

			//Get the subplatformID from the SubPlatformCheckAPI attribute
			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}

			List<Person> persons = itemManager.GetAllPersonsForSubplatform(suplatformID).ToList();

			if (!persons.Any()) return NotFound();

			List<string> districts = persons.Select(person => person.District).Distinct().ToList();

			List<List<ItemViewModels.PersonViewModel>> results = new List<List<ItemViewModels.PersonViewModel>>();
			districts.ForEach(district =>
			{
				List<Person> top3persons = persons.Where(person => person.District == district)
												  .OrderByDescending(person => person.NumberOfMentions)
												  .Take(3)
												  .ToList();

				List<ItemViewModels.PersonViewModel> top3 = Mapper.Map(top3persons, new List<ItemViewModels.PersonViewModel>());
				List<ItemDTO> top3items = Mapper.Map(top3persons, new List<ItemDTO>());
				for (int i = 0; i < top3.Count; i++)
				{
					top3.ElementAt(i).Item = top3items.ElementAt(i);
				}
				results.Add(top3);
			});

			return Ok(results);
		}
	}
}