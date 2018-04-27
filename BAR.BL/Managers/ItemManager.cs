using System;
using BAR.DAL;
using BAR.BL.Domain.Items;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using System.Linq;
using BAR.BL.Domain.Users;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using BAR.BL.Domain.Core;
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

			if (allInfoForId.Count() > 0)
			{
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
		/// Returns an item for a specifig itemId including the attached subplatform.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public Item GetItemWithSubPlatform(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItemWithSubPlatform(itemId);
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
		/// Returns all (undeleted) people of the whole system
		/// </summary>
		public IEnumerable<Item> GetAllPersons()
		{
			InitRepo();
			return itemRepo.ReadAllPersons().AsEnumerable();
		}

		/// <summary>
		/// Returns all (undeleted) organisations of the whole system
		/// </summary>
		public IEnumerable<Item> GetAllOrganisations()
		{
			InitRepo();
			return itemRepo.ReadAllOraginsations().AsEnumerable();

		}

		/// <summary>
		/// Creates a new item based on the given parameters
		/// 
		/// NOTE
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Item AddItem(ItemType itemType, string name, string description = "", string function = "", Category category = null,
			string district = null, string level = null, string site = null, Gender gender = Gender.OTHER, string position = null, DateTime? dateOfBirth = null)
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
						District = district,
						Level = level,
						Gender = gender,
						Site = site,
						DateOfBirth = dateOfBirth,
						Position = position,
						SocialMediaNames = new List<SocialMediaName>(),
					};
					break;
				case ItemType.Organisation:
					item = new Organisation()
					{
						Site = site,
						SocialMediaUrls = new List<SocialMediaName>()
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
			item.NumberOfFollowers = 0;
			item.TrendingPercentage = 0.0;
			item.Baseline = 0.0;
			item.Deleted = false;
			item.Informations = new List<Information>();
            item.ItemWidgets = new List<Widget>();

			itemRepo.CreateItem(item);

			return item;
		}

		/// <summary>
		/// Gives every item default widgets
		/// </summary>
		private void GenerateDefaultItemWidgetsForItems(IEnumerable<Item> items)
		{
			foreach (Item item in items)
			{
				item.ItemWidgets = GenerateDefaultItemWidgets(item.Name, item.ItemId);
				itemRepo.UpdateItem(item);
			}
		}

		/// <summary>
		/// Generates dafault widgets based on the itemid
		/// </summary>
		private List<Widget> GenerateDefaultItemWidgets(string name, int itemId)
		{
			List<Widget> lijst = new List<Widget>();
			WidgetManager widgetManager = new WidgetManager();

			ItemWidget widget = (ItemWidget)widgetManager.CreateWidget(WidgetType.GraphType, name + " popularity", 1, 1, rowspan: 12, colspan: 6);
			lijst.Add(widget);

			widgetManager.AddItemToWidget(widget.WidgetId, itemId);
			return lijst;
		}

		/// <summary>
		/// Returns all (undeleted) themes of the whole system
		/// </summary>
		public IEnumerable<Item> GetAllThemes(int subplatformId)
		{
			InitRepo();
			return itemRepo.ReadAllThemes()
				.Where(item => item.SubPlatform.SubPlatformId == subplatformId)
				.AsEnumerable();
		}

		/// <summary>
		/// Returns all people for specific subplatform
		/// </summary>
		/// <param name="subPlatformName"></param>
		/// <returns></returns>
		public IEnumerable<Item> GetAllPersonsForSubplatform(int subPlatformID)
		{
			return GetAllPersons()
				.Where(item => item.Deleted == false)
				.Where(item => item.SubPlatform.SubPlatformId.Equals(subPlatformID));
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
		//public Item ChangeItemDescription(int itemId, string description)
		//{
		//	InitRepo();

		//	//Get item
		//	Item itemToUpdate = GetItem(itemId);
		//	if (itemToUpdate == null) return null;

		//	//Update item
		//	itemToUpdate.Description = description;
		//	itemToUpdate.LastUpdated = DateTime.Now;

		//	//Update database
		//	itemRepo.UpdateItem(itemToUpdate);
		//	return itemToUpdate;
		//}

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
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) itemRepo = new ItemRepository();
			else itemRepo = new ItemRepository(uowManager.UnitOfWork);
		}

		/// <summary>
		/// Gives back an item with all the widgets
		/// </summary>
		public Item GetItemWithAllWidgets(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItemWithWidgets(itemId);
		}
		/// <summary>
		/// Gets person with given name.
		/// </summary>
		public Item GetPerson(string personName)
		{
			InitRepo();
			return itemRepo.ReadPerson(personName);
		}

		/// <summary>
		/// Convert httppostedfilebase to string (used for json parsing).
		/// </summary>
		public string ConvertPfbToString(HttpPostedFileBase pfb)
		{
			string json = string.Empty;

			using (BinaryReader b = new BinaryReader(pfb.InputStream))
			{
				byte[] binData = b.ReadBytes(pfb.ContentLength);
				json = System.Text.Encoding.UTF8.GetString(binData);
			}
			return json;
		}

		/// <summary>
		/// Calls handling methods for a correct json transaction.
		/// </summary>
		public bool ImportJson(string json, int subPlatformID)
		{
			CheckOrganisations(json, subPlatformID);
			return AddItemsFromJson(json, subPlatformID);
		}

		/// <summary>
		/// Checks if organisations used in json already exist, if not they will be made.
		/// </summary>
		private void CheckOrganisations(string json, int subPlatformID)
		{
			uowManager = new UnitOfWorkManager();

			InitRepo();

			ISubplatformManager subplatformManager = new SubplatformManager(uowManager);
			SubPlatform subPlatform = subplatformManager.GetSubPlatform(subPlatformID);

			dynamic deserializedJson = JsonConvert.DeserializeObject(json);
			List<Item> organisations = new List<Item>();

			for (int i = 0; i < deserializedJson.Count; i++)
			{
				string name = deserializedJson[i].organisation;
				Item organisation = itemRepo.ReadOrganisation(name);

				if (organisation == null)
				{
					organisation = new Organisation()
					{
						ItemType = ItemType.Organisation,
						Name = name,
						CreationDate = DateTime.Now,
						LastUpdatedInfo = DateTime.Now,
						LastUpdated = DateTime.Now,
						NumberOfFollowers = 0,
						TrendingPercentage = 0.0,
						Baseline = 0.0,
						Informations = new List<Information>(),
						SocialMediaUrls = new List<SocialMediaName>(),
						SubPlatform = subPlatform
					};
					itemRepo.CreateItem(organisation);
					uowManager.Save();
					organisations.Add(organisation);
				}
			}
			GenerateDefaultItemWidgetsForItems(organisations);
			uowManager = null;
		}

		/// <summary>
		/// Reads json and makes item objects which will be saved afterwards into the database.
		/// </summary>
		private bool AddItemsFromJson(string json, int subPlatformID)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);

			//Needs to be in memory to gain preformance
			IUserManager userManager = new UserManager(uowManager);
			IDataManager dataManager = new DataManager(uowManager);
			ISubplatformManager subplatformManager = new SubplatformManager(uowManager);
			SubPlatform subPlatform = subplatformManager.GetSubPlatform(subPlatformID);
			IEnumerable<Area> areas = userManager.GetAreas();
			IEnumerable<Source> sources = dataManager.GetAllSources();
			IEnumerable<Item> organisations = GetAllOrganisations();
			IEnumerable<Item> persons = GetAllPersons();

			List<Item> items = new List<Item>();

			for (int i = 0; i < deserializedJson.Count; i++)
			{
				string fullname = deserializedJson[i].full_name;
				if (persons.Where(person => person.SubPlatform.SubPlatformId == subPlatformID)
					.Where(x => x.Name.Equals(fullname)).SingleOrDefault() == null)
				{
					string gender = deserializedJson[i].gender;
					string postalCode = deserializedJson[i].postal_code;
					string organisation = deserializedJson[i].organisation;
					string twitter = deserializedJson[i].twitter;
					string facebook = deserializedJson[i].facebook;
					string stringDate = Convert.ToString(deserializedJson[i].dateOfBirth);
					string town = deserializedJson[i].town;
                    string level = deserializedJson[i].level;
                    string site = deserializedJson[i].site;
                    string district = deserializedJson[i].district;
                    string position = deserializedJson[i].position;

                    Gender personGender = (gender == "M") ? Gender.MAN : Gender.WOMAN;
					DateTime? dateOfBirth = DateTime.ParseExact(stringDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

					Person person = (Person) AddItem(itemType: ItemType.Person, name: fullname, gender: personGender, district: district,
						level: level, site: site, position: position, dateOfBirth: dateOfBirth);
					person.SubPlatform = subPlatform;	person.Area = areas.Where(x => x.PostalCode.Equals(postalCode) && x.Residence.ToLower().Equals(town.ToLower())).SingleOrDefault();
					
					if (!string.IsNullOrEmpty(twitter))
					{
						SocialMediaName twitterSocial = new SocialMediaName()
						{
							Username = twitter,
							Source = sources.Where(x => x.Name.Equals("Twitter")).SingleOrDefault()
						};
						person.SocialMediaNames.Add(twitterSocial);
					}
					if (!string.IsNullOrEmpty(facebook))
					{
						SocialMediaName facebookSocial = new SocialMediaName()
						{
							Username = facebook,
							Source = sources.Where(x => x.Name.Equals("Facebook")).SingleOrDefault()
						};
						person.SocialMediaNames.Add(facebookSocial);
					}
					person.Organisation = (Organisation) organisations.Where(x => x.Name.Equals(organisation)).SingleOrDefault();

					items.Add(person);
				}
			}

			if (items.Count > 0)
			{
				itemRepo.CreateItems(items);
				uowManager.Save();
				GenerateDefaultItemWidgetsForItems(items);
				return true;
			}
			return false;
		}
	}
}
