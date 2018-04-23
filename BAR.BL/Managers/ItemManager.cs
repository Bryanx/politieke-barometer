using System;
using BAR.DAL;
using BAR.BL.Domain.Items;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using System.Linq;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for managing:
	/// Organisations
	/// Persons
	/// Themes
	/// </summary>
	public class ItemManager : IItemManager
	{
		private IItemRepository itemRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public ItemManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Adjusts the baseline of the given item and 
		/// Adjusts the trendingpercentage of the given item.
		/// </summary>
		public void DetermineTrending(int itemId)
		{
			InitRepo();

			DataManager dataManager = new DataManager();
			IEnumerable<Information> allInfoForId = dataManager.GetInformationsForItemid(itemId);

			DateTime earliestInfoDate = allInfoForId.Min(item => item.CreationDate).Value;
			DateTime lastInfoDate = allInfoForId.Max(item => item.CreationDate).Value;

			int period = (lastInfoDate - earliestInfoDate).Days;

			if (period == 0) period = 1;

			int aantalBaseline = dataManager.GetNumberInfo(itemId, earliestInfoDate);
			int aantalTrending = dataManager.GetNumberInfo(itemId, lastInfoDate.AddDays(-1));

			// Calculate the baseline = number of information / number of days from the last update until now
			double baseline = Convert.ToDouble(aantalBaseline) / Convert.ToDouble(period);

			// Calculate the trendingpercentage = baseline / number of days from the last update until now.
			double trendingPer = Convert.ToDouble(aantalTrending) / baseline;

			itemRepo.UpdateItemTrending(itemId, baseline, trendingPer);
			itemRepo.UpdateLastUpdated(itemId, DateTime.Now);
		}

		/// <summary>
		/// Gives back te most trending items
		/// the number of trending items depends on the
		/// number that you give via the parameter
		/// </summary>
		public IEnumerable<Item> GetMostTrendingItems(int numberOfItems = 5)
		{
			//Order the items by populairity
			IEnumerable<Item> itemsOrderd = GetAllItems()
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();

			//Get the first items out of the list
			List<Item> itemsOrderdMostPopulair = new List<Item>();
			for (int i = 0; i < numberOfItems; i++)
			{
				if (i <= itemsOrderd.Count()) itemsOrderdMostPopulair.Add(itemsOrderd.ElementAt(i));
			}

			return itemsOrderdMostPopulair.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of the most trending items
		/// for a specific type
		/// the number of items depends on the parameter "numberOfItems"
		/// </summary>
		public IEnumerable<Item> GetMostTrendingItemsForType(ItemType type, int numberOfItems = 5)
		{
			//order the items by populairity
			IEnumerable<Item> itemsOrderd = GetAllItems().Where(item => item.ItemType == type)
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();

			//Get the first items out of the list
			List<Item> itemsOrderdMostPopulair = new List<Item>();
			for (int i = 0; i < numberOfItems; i++)
			{
				if (i <= itemsOrderd.Count()) itemsOrderdMostPopulair.Add(itemsOrderd.ElementAt(i));
			}

			return itemsOrderdMostPopulair.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of the most trending items
		/// based on the userId.
		/// </summary>
		public IEnumerable<Item> GetMostTredningItemsForUser(string userId, int numberOfItems = 5)
		{
			//Get items for userId and order items from user
			//We need to get every item of the subscription of a specefic user
			SubscriptionManager subManager = new SubscriptionManager();
			List<Item> itemsFromUser = new List<Item>();
			foreach (Subscription sub in subManager.GetSubscriptionsWithItemsForUser(userId))
			{
				itemsFromUser.Add(sub.SubscribedItem);
			}

			//Order items
			IEnumerable<Item> itemsOrderd = itemsFromUser
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();

			//Get the first items out of the list
			List<Item> itemsOrderdMostPopulair = new List<Item>();
			for (int i = 0; i < numberOfItems; i++)
			{
				if (i <= itemsOrderd.Count()) itemsOrderdMostPopulair.Add(itemsOrderd.ElementAt(i));
			}

			return itemsOrderd;
		}

		/// <summary>
		/// Gives back a list of the most trending items
		/// for a specific type and user
		/// the number of items depends on the parameter "numberOfItems"
		/// </summary>
		public IEnumerable<Item> GetMostTredningItemsForUserAndItemType(string userId, ItemType type, int numberOfItems = 5)
		{
			//Get items for userId and order items from user
			//We need to get every item of the subscription of a specefic user
			SubscriptionManager subManager = new SubscriptionManager();
			List<Item> itemsFromUser = new List<Item>();
			foreach (Subscription sub in subManager.GetSubscriptionsWithItemsForUser(userId))
			{
				itemsFromUser.Add(sub.SubscribedItem);
			}

			//Order items
			IEnumerable<Item> itemsOrderd = itemsFromUser.Where(item => item.ItemType == type)
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();

			//Get the first items out of the list
			List<Item> itemsOrderdMostPopulair = new List<Item>();
			for (int i = 0; i < numberOfItems; i++)
			{
				if (i <= itemsOrderd.Count()) itemsOrderdMostPopulair.Add(itemsOrderd.ElementAt(i));
			}

			return itemsOrderd;
		}

		/// <summary>
		/// Returns an item from a specific itemId.
		/// </summary>
		public Item GetItem(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItem(itemId);
		}

		/// <summary>
		/// Returns an item with widgets.
		/// </summary>
		public Item GetItemWithWidgets(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItemWithWidgets(itemId);
		}

		/// <summary>
		/// Gives back all the items of a specific type
		/// </summary>
		public IEnumerable<Item> GetItemsForType(ItemType type)
		{
			IEnumerable<Item> items = GetAllItems();
			return items.Where(item => item.ItemType == type).AsEnumerable();
		}

		/// <summary>
		/// Gets the trending percentage of a specific item
		/// </summary>
		public double GetTrendingPer(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItem(itemId).TrendingPercentage;
		}

		/// <summary>
		/// Returns a list of all items (deleted and undeleted).
		/// </summary>
		public IEnumerable<Item> GetAllItems()
		{
			InitRepo();
			return itemRepo.ReadAllItems();
		}

		/// <summary>
		/// Returns all (undeleted) people
		/// </summary>
		public IEnumerable<Item> GetAllPersons()
		{
			return GetAllItems().Where(item => item is Person).Where(item => item.Deleted == false).ToList();
		}

		/// <summary>
		/// Returns all (undeleted) organisations
		/// </summary>
		public IEnumerable<Item> GetAllOrganisations()
		{
			return GetAllItems().Where(item => item is Organisation).Where(item => item.Deleted == false);

		}

		/// <summary>
		/// Creates a new item based on the given parameters
		/// </summary>
		public Item CreateItem(ItemType itemType, string name, string description = "", string function = "", Category category = null)
		{
			InitRepo();

			//the switch statement will determine if we need to make a
			//Organisation, person or theme.
			Item item;
			switch (itemType)
			{
				case ItemType.Person:
					item = new Person()
					{
						SocialMediaUrls = new List<SocialMediaUrl>(),
						Function = function
					};
					break;
				case ItemType.Organisation:
					item = new Organisation()
					{
						SocialMediaUrls = new List<SocialMediaUrl>()
					};
					break;
				case ItemType.Theme:
					item = new Theme()
					{
						Category = category
					};
					break;
				default:
					item = null;
					break;
			}

			if (item == null) return null;
			item.ItemType = itemType;
			item.Name = name;
			item.CreationDate = DateTime.Now;
			item.LastUpdatedInfo = DateTime.Now;
			item.LastUpdated = DateTime.Now;
			item.Description = description;
			item.NumberOfFollowers = 0;
			item.TrendingPercentage = 0.0;
			item.Baseline = 0.0;
			item.Informations = new List<Information>();
			item.ItemWidgets = GenerateDefaultItemWidgets(name);

			itemRepo.CreateItem(item);
			return item;
		}

		public List<ItemWidget> GenerateDefaultItemWidgets(string name)
		{
			List<ItemWidget> lijst = new List<ItemWidget>();
			WidgetManager widgetManager = new WidgetManager();
			ItemWidget widget = (ItemWidget)widgetManager.CreateWidget(WidgetType.GraphType, name + " popularity", 1, 1, null, 12, 6);
			lijst.Add(widget);
			return lijst;
		}

		/// <summary>
		/// Updates the name of a given item.
		/// </summary>
		public Item ChangeItemName(int itemId, string name)
		{
			InitRepo();

			//Get item
			Item itemToUpdate = GetItem(itemId);
			if (itemToUpdate == null) return null;

			//Update item
			itemToUpdate.Name = name;
			itemToUpdate.LastUpdated = DateTime.Now;

			//Update database
			itemRepo.UpdateItem(itemToUpdate);
			return itemToUpdate;
		}

		/// <summary>
		/// Updates the description of a given item.
		/// </summary>
		public Item ChangeItemDescription(int itemId, string description)
		{
			InitRepo();

			//Get item
			Item itemToUpdate = GetItem(itemId);
			if (itemToUpdate == null) return null;

			//Update item
			itemToUpdate.Description = description;
			itemToUpdate.LastUpdated = DateTime.Now;

			//Update database
			itemRepo.UpdateItem(itemToUpdate);
			return itemToUpdate;
		}

		/// <summary>
		/// Changes an item to non-active or active
		/// </summary>
		public Item ChangeItemActivity(int itemId)
		{
			InitRepo();

			//Get item
			Item itemToUpdate = itemRepo.ReadItem(itemId);
			if (itemToUpdate == null) return null;

			//Change item (toggle Deleted)
			itemToUpdate.Deleted = !itemToUpdate.Deleted;

			//Update database
			itemRepo.UpdateItem(itemToUpdate);
			return itemToUpdate;
		}

		/// <summary>
		/// Removes an item from the database.
		/// </summary>
		public void RemoveItem(int itemId)
		{
			InitRepo();
			Item itemToRemove = GetItem(itemId);
			if (itemToRemove != null) itemRepo.DeleteItem(itemToRemove);
		}

		/// <summary>
		/// Returns a person based on the persons name
		/// </summary>
		public Item GetPerson(string personName)
		{
			InitRepo();
			return itemRepo.ReadPerson(personName);
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) itemRepo = new ItemRepository();
			else itemRepo = new ItemRepository(uowManager.UnitOfWork);
		}		
	}
}
