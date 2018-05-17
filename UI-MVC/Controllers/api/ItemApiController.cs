﻿using System;
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
using WebGrease.Css.Extensions;
using BAR.BL.Domain.Core;
using WebGrease.Css.Extensions;

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
		/// Gets top 3 trending items with details for specific type 
		/// </summary>
		[HttpGet]
		[Route("api/GetTopTrendingItems/{itemType}")]
		public IHttpActionResult GetTopTrendingItems(int itemType)
		{
			ItemType type = (ItemType) itemType;
			itemManager = new ItemManager();
			List<Item> lijst = new List<Item>();

			if (type == ItemType.Person){
				itemManager.GetMostTrendingItemsForType(ItemType.Person, 3)
					.ForEach(i => lijst.Add(itemManager.GetPersonWithDetails(i.ItemId)));
			} else if (type == ItemType.Organisation){
				itemManager.GetMostTrendingItemsForType(ItemType.Organisation, 3)
					.ForEach(i => lijst.Add(itemManager.GetOrganisationWithDetails(i.ItemId)));
			} else {
				itemManager.GetMostTrendingItemsForType(ItemType.Theme, 3)
					.ForEach(i => lijst.Add(itemManager.GetThemeWithDetails(i.ItemId)));
			}

			return Ok(Mapper.Map(lijst, new List<Item>()));
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
		/// Retrieves more people from the same organisation.
		/// </summary>
		[HttpGet]
		[Route("api/GetMorePeopleFromOrg/{itemId}")]
		public IHttpActionResult GetMorePeopleFromOrg(string itemId)
		{
			itemManager = new ItemManager();

			Person person = itemManager.GetPersonWithDetails(Int32.Parse(itemId));
			if(person == null)
			{
				return Ok(Mapper.Map(new List<Person>(), new List<ItemDTO>()));
			} else
			{
				int orgId = person.Organisation.ItemId;
				List<Person> items = itemManager.GetAllPersons()
					.Where(p => p.Organisation.ItemId == orgId)
					.OrderByDescending(p => p.NumberOfMentions)
					.Take(6).ToList();
				return Ok(Mapper.Map(items, new List<ItemDTO>()));
			}
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
			try
			{
				List<Person> items = itemManager.GetAllPersons().Where(p => p.Organisation.ItemId == orgId).ToList();
				return Ok(Mapper.Map(items, new List<ItemDTO>()));
			} catch(Exception e)
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
		public IHttpActionResult GetStories(int itemId) {
			IDataManager dataManager = new DataManager();
			List<string> urls = dataManager.GetUrlsForItem(itemId).ToList();
			return Ok(urls.Distinct().Take(9));
		}
	}
}