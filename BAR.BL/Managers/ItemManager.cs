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
using Microsoft.AspNet.Identity;

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

			//Get item
			Item itemToUpdate = itemRepo.ReadItem(itemId);
			if (itemToUpdate == null) return;

			//Get all informations
			IEnumerable<Information> infos = new DataManager().GetInformationsForItemid(itemId);
			if (infos == null || infos.Count() == 0) return;
	
			//Calculate new baseline (avarage of the last 30 days)
			int infosOld = infos.Where(info => info.CreationDate.Value < DateTime.Now && info.CreationDate.Value >= DateTime.Now.AddDays(-30)).Count();
			if (infosOld != 0) itemToUpdate.Baseline = Math.Round((double) (infosOld / 30));

			//Determine trending percentage (avarage of the last 3 days)
			int infosNew = infos.Where(info => info.CreationDate.Value.ToString("dd-MM-yy").Equals(DateTime.Now.ToString("dd-MM-yy"))
								|| info.CreationDate.Value >= DateTime.Now.AddDays(-3))
								.Count();
			if (infosNew != 0)
			{
				//If trendingper is 0 then it will be devided by 1
				//Bescause deviding by zero gives back an error
				double trendingPer;
				if (itemToUpdate.Baseline == 0) trendingPer = infosNew / 1;
				else trendingPer = infosNew / itemToUpdate.Baseline;

				if (trendingPer > 0)
				{
					itemToUpdate.TrendingPercentage = Math.Round(trendingPer, 2);
					new SubscriptionManager().GenerateAlerts(itemToUpdate.ItemId);
				}
			}

			//Save changes
			itemRepo.UpdateItem(itemToUpdate);
		}

		/// <summary>
		/// Gives back te most trending items
		/// the number of trending items depends on the
		/// number that you give via the parameter
		/// </summary>
		public IEnumerable<Item> GetMostTrendingItems(int numberOfItems = 5, bool useWithOldData = false)
		{
			//Order the items by populairity
			IEnumerable<Item> itemsOrderd = GetAllItems();
			if (!useWithOldData)
			{
				itemsOrderd = itemsOrderd.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}
			else
			{
				itemsOrderd = itemsOrderd.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}

			return itemsOrderd.Take(numberOfItems).AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of the most trending items
		/// for a specific type
		/// the number of items depends on the parameter "numberOfItems"
		/// </summary>
		public IEnumerable<Item> GetMostTrendingItemsForType(ItemType type, int numberOfItems = 5, bool useWithOldData = false)
		{
			//order the items by populairity
			IEnumerable<Item> itemsOrderd = GetAllItems();
			if (!useWithOldData)
			{
				itemsOrderd = itemsOrderd.Where(item => item.ItemType == type)
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}
			else
			{
				itemsOrderd = itemsOrderd.Where(item => item.ItemType == type)
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}

			return itemsOrderd.Take(numberOfItems).AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of the most trending items
		/// based on the userId.
		/// </summary>
		public IEnumerable<Item> GetMostTrendingItemsForUser(string userId, int numberOfItems = 5, bool useWithOldData = false)
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
			IEnumerable<Item> itemsOrderd;
			if (!useWithOldData)
			{
				itemsOrderd = itemsFromUser.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}
			else
			{
				itemsOrderd = itemsFromUser.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}

			return itemsOrderd.Take(numberOfItems).AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of the most trending items
		/// for a specific type and user
		/// the number of items depends on the parameter "numberOfItems"
		/// </summary>
		public IEnumerable<Item> GetMostTrendingItemsForUserAndItemType(string userId, ItemType type, int numberOfItems = 5, bool useWithOldData = false)
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
			IEnumerable<Item> itemsOrderd;
			if (!useWithOldData)
			{
				itemsOrderd = itemsFromUser.Where(item => item.ItemType == type)
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}
			else
			{
				itemsOrderd = itemsFromUser.Where(item => item.ItemType == type)
				.OrderBy(item => item.TrendingPercentage).AsEnumerable();
			}

			return itemsOrderd.Take(numberOfItems).AsEnumerable();
		}

		/// <summary>
		/// If the last time that the old trending percentage of the item was updated 7 days ago,
		/// then the old trending percentage will be updated.
		/// </summary>
		public void UpdateWeeklyReviewData(int platformId)
		{
			//Get timestamp
			DateTime? lastUpdated = new SubplatformManager().GetSubPlatform(platformId).LastUpdatedWeeklyReview;

			//Get all items
			IEnumerable<Item> items = itemRepo.ReadAllItemsWithPlatforms();
			if (items == null || items.Count() == 0) return;

			//Refresh old data
			if (lastUpdated == null || lastUpdated < DateTime.Now.AddDays(-7))
			{
				foreach (Item item in items) item.NumberOfMentionsOld = item.NumberOfMentions;
			}

			//Update database
			itemRepo.UpdateItems(items);
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
		/// Returns a person with all personal details.
		/// </summary>
		public Person GetPersonWithDetails(int itemId)
		{
			InitRepo();
			return itemRepo.ReadPersonWithDetails(itemId);
		}

		/// <summary>
		/// Returns an organisation with all personal details.
		/// </summary>
		public Organisation GetOrganisationWithDetails(int itemId)
		{
			InitRepo();
			return itemRepo.ReadOrganisationWithDetails(itemId);
		}

		/// <summary>
		/// Returns a theme with all the Theme keywords
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public Theme GetThemeWithDetails(int itemId)
		{
			InitRepo();
			return itemRepo.ReadThemeWithDetails(itemId);
		}

		/// <summary>
		/// Returns an item for a specifig itemId including the attached subplatform.
		/// </summary>
		/// <returns></returns>
		public Item GetItemWithSubPlatform(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItemWithPlatform(itemId);
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
			return itemRepo.ReadAllItemsWithPlatforms().AsEnumerable();
		}

		/// <summary>
		/// Returns all people of the whole system
		/// </summary>
		public IEnumerable<Person> GetAllPersons()
		{
			InitRepo();
			return itemRepo.ReadAllPersonsWithPlatforms();
		}

		/// <summary>
		/// Returns all (undeleted) people for an organisation
		/// </summary>
		/// <param name="organisationId"></param>
		/// <returns></returns>
		public IEnumerable<Person> GetAllPersonsForOrganisation(int organisationId)
		{
			InitRepo();
			return itemRepo.ReadAllPersonsForOrganisation(organisationId).AsEnumerable();
		}

		/// <summary>
		/// Returns all (undeleted) organisations of the whole system
		/// </summary>
		public IEnumerable<Organisation> GetAllOrganisations()
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
		public Item AddItem(ItemType itemType, string name, string description = "", string function = "",
			string district = null, string level = null, string site = null, Gender gender = Gender.OTHER, string position = null, DateTime? dateOfBirth = null, List<Keyword> keywords = null)
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
					SocialMediaNames = new List<SocialMediaName>()
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
					if(keywords != null)
					{
						item = new Theme()
						{
							Keywords = keywords
						};
					} else
					{
						item = new Theme()
						{
							Keywords = new List<Keyword>()
							{

							}
						};
					}
					
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
			//needs to be null to do a check later on: see updateItemTrending in itemManager for details
			item.NumberOfFollowers = 0;
			item.TrendingPercentage = 0.0;
			item.NumberOfMentions = 0;
			item.NumberOfMentionsOld = 0;
			item.Baseline = 0.0;
			item.Deleted = false;
			item.Baseline = 10.0;
			item.Informations = new List<Information>();
			item.ItemWidgets = new List<Widget>();

			itemRepo.CreateItem(item);

			return item;
		}

		/// <summary>
		/// Generates dafault widgets based on the itemid
		/// </summary>
		public void GenerateDefaultItemWidgets(string name, int itemId)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			WidgetManager widgetManager = new WidgetManager(uowManager);
			List<Widget> itemWidgets = new List<Widget>();
			List<int> widgetIds = new List<int>();
			List<PropertyTag> proptags;

			//Get item
			Item item = GetItemWithAllWidgets(itemId);

			//1st widget
			proptags = new List<PropertyTag>();
			proptags.Add(new PropertyTag()
			{
				Name = "Mentions"
			});

			ItemWidget widget1 = (ItemWidget)widgetManager.AddWidget(WidgetType.GraphType, name + " popularity", 1, 1, proptags: proptags, graphType: GraphType.LineChart, rowspan: 12, colspan: 6);
			itemWidgets.Add(widget1);
			widgetIds.Add(widget1.WidgetId);

			//2nd widget
			proptags = new List<PropertyTag>();
			proptags.Add(new PropertyTag()
			{
				Name = "Gender"
			});
			ItemWidget widget2 = (ItemWidget)widgetManager.AddWidget(WidgetType.GraphType, name + " gender comparison ", 1, 1, proptags: proptags, graphType: GraphType.PieChart, rowspan: 6, colspan: 6);
			itemWidgets.Add(widget2);
			widgetIds.Add(widget2.WidgetId);

			//3rd widget
			proptags = new List<PropertyTag>();
			proptags.Add(new PropertyTag()
			{
				Name = "Age"
			});
			ItemWidget widget3 = (ItemWidget)widgetManager.AddWidget(WidgetType.GraphType, name + " age comparison", 1, 1, proptags: proptags, graphType: GraphType.DonutChart, rowspan: 6, colspan: 6);
			itemWidgets.Add(widget3);
			widgetIds.Add(widget3.WidgetId);

			//4th widget
			proptags = new List<PropertyTag>();
			proptags.Add(new PropertyTag()
			{
				Name = "Education"
			});
			ItemWidget widget4 = (ItemWidget)widgetManager.AddWidget(WidgetType.GraphType, name + " education comparison", 1, 1, proptags: proptags, graphType: GraphType.PieChart, rowspan: 6, colspan: 6);
			itemWidgets.Add(widget4);
			widgetIds.Add(widget4.WidgetId);

			//5th widget
			proptags = new List<PropertyTag>();
			proptags.Add(new PropertyTag()
			{
				Name = "Personality"
			});
			ItemWidget widget5 = (ItemWidget)widgetManager.AddWidget(WidgetType.GraphType, name + " personality comparison", 1, 1, proptags: proptags, graphType: GraphType.BarChart, rowspan: 6, colspan: 6);
			itemWidgets.Add(widget5);
			widgetIds.Add(widget5.WidgetId);

			//Link widgets to item & save changes to database
			item.ItemWidgets = itemWidgets;
			itemRepo.UpdateItem(item);
			uowManager.Save();
			uowManager = null;
		}

		/// <summary>
		/// Returns all (undeleted) themes of the whole system
		/// </summary>
		public IEnumerable<Theme> GetAllThemes()
		{
			InitRepo();
			return itemRepo.ReadAllThemes().AsEnumerable();
		}

		/// <summary>
		/// Returns all people for specific subplatform
		/// </summary>
		/// <param name="subPlatformName"></param>
		/// <returns></returns>
		public IEnumerable<Person> GetAllPersonsForSubplatform(int subPlatformID)
		{
			return GetAllPersons()
				.Where(item => item.Deleted == false)
				.Where(item => item.SubPlatform.SubPlatformId.Equals(subPlatformID));
		}

		/// <summary>
		/// Returns all organisations for specific subplatform
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Item> GetAllOrganisationsForSubplatform(int subPlatformID)
		{
			return GetAllOrganisations()
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

			//Update database
			itemRepo.UpdateItem(itemToUpdate);
			return itemToUpdate;
		}

		/// <summary>
		/// Updates the organisation of a given person
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="organisationId"></param>
		/// <returns></returns>
		public Person ChangePersonOrganisation(int personId, int organisationId)
		{
			uowManager = new UnitOfWorkManager();

			InitRepo();

			Organisation org = this.GetOrganisationWithDetails(organisationId);
			//Get item
			Person personToUpdate = GetPersonWithDetails(personId);

			if (personToUpdate == null) return null;

			//Update item
			personToUpdate.Organisation = org;

			//Update database
			itemRepo.UpdatePerson(personToUpdate);

			uowManager.Save();
			uowManager = null;
			return personToUpdate;
		}


		/// <summary>
		/// Adds socialmedia names to person
		/// </summary>
		/// <param name="person"></param>
		/// <returns></returns>
		public Person ChangePersonSocialMedia(int personId, string twitter, string facebook)
		{
			uowManager = new UnitOfWorkManager();
			IDataManager dataManager = new DataManager(uowManager);
			InitRepo();

			Source twitterSource = dataManager.GetSource("Twitter");
			Source facebookSource = dataManager.GetSource("Facebook");

			//Get item
			Person personToUpdate = GetPersonWithDetails(personId);

			if (personToUpdate == null) return null;

			//Update person with twitter and facebook url's
			personToUpdate.SocialMediaNames.Add(new SocialMediaName()
			{
				Source = twitterSource,
				Username = twitter
			});

			personToUpdate.SocialMediaNames.Add(new SocialMediaName()
			{
				Source = facebookSource,
				Username = facebook
			});

			//Update database
			itemRepo.UpdatePerson(personToUpdate);

			uowManager.Save();
			uowManager = null;
			return personToUpdate;
		}

		/// <summary>
		/// Updates the subplatform of a given item
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="suplatformId"></param>
		/// <returns></returns>
		public Item ChangeItemPlatform(int itemId, int subplatformId)
		{
			uowManager = new UnitOfWorkManager();
			ISubplatformManager subManager = new SubplatformManager(uowManager);
			
			InitRepo();

			SubPlatform subPlatform = subManager.GetSubPlatform(subplatformId);
			//Get item
			Item itemToUpdate = GetItemWithSubPlatform(itemId);

			if (itemToUpdate == null) return null;

			//Update item
			itemToUpdate.SubPlatform = subPlatform;
			//Update database
			itemRepo.UpdateItem(itemToUpdate);

			uowManager.Save();
			uowManager = null;
			return itemToUpdate;
		}

		/// <summary>
		/// Updates the site of a given Organisation
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		public Organisation ChangeOrganisation(int itemId, string site)
		{
			InitRepo();

			//Get item
			Organisation orgToUpdate = GetOrganisationWithDetails(itemId);

			if (orgToUpdate == null) return null;

			//Update item
			orgToUpdate.Site = site;

			//Update database
			itemRepo.UpdateItem(orgToUpdate);
			return orgToUpdate;
		}

		/// <summary>
		/// Updates the site of a given person
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		public Person ChangePerson(int itemId, string site)
		{
			InitRepo();

			//Get item
			Person personToUpdate = GetPersonWithDetails(itemId);

			if (personToUpdate == null) return null;

			//Update item
			personToUpdate.Site = site;

			//Update database
			itemRepo.UpdateItem(personToUpdate);
			return personToUpdate;
		}

		/// <summary>
		/// Updates a person.
		/// </summary>
		public Person ChangePerson(int itemId, DateTime birthday, Gender gender, string position, string district)
		{
			InitRepo();

			//Get item
			Person personToUpdate = GetPersonWithDetails(itemId);

			if (personToUpdate == null) return null;

			//Update item
			personToUpdate.DateOfBirth = birthday;
			personToUpdate.Gender = gender;
			personToUpdate.Position = position;
			personToUpdate.District = district;

			//Update database
			itemRepo.UpdateItem(personToUpdate);

			return personToUpdate;
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
		public Item GetItemByName(string name) {
			InitRepo();
			return itemRepo.ReadItemByName(name);
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
		/// Import all the themes from the json file
		/// </summary>
		/// <param name="json"></param>
		/// <param name="subPlatformID"></param>
		/// <returns></returns>
		public bool ImportThemes(string json, int subPlatformID)
		{
			return AddThemesFromJson(json, subPlatformID);
		}

		/// <summary>
		/// Check if theme exists, if not add it to the Database
		/// </summary>
		/// <param name="json"></param>
		/// <param name="subPlatformID"></param>
		private bool AddThemesFromJson(string json, int subPlatformID)
		{
			uowManager = new UnitOfWorkManager();

			InitRepo();

			ISubplatformManager subplatformManager = new SubplatformManager(uowManager);
			SubPlatform subPlatform = subplatformManager.GetSubPlatform(subPlatformID);
			IEnumerable<Item> themes = GetAllThemes();
			List<Item> newThemes = new List<Item>();

			dynamic deserializedJson = JsonConvert.DeserializeObject(json);

			for (int i = 0; i < deserializedJson.Count; i++)
			{
				string name = deserializedJson[i].name;

				Theme theme = null;

				List<Keyword> keywords = new List<Keyword>();

				for (int j = 0; j < deserializedJson[i].keywords.Count; j++)
				{
					keywords.Add(new Keyword()
					{
						Name = deserializedJson[i].keywords[j]
					});
				}

				if (themes
				.Where(t => t.SubPlatform.SubPlatformId == subPlatformID)
				.Where(x => x.Name.Equals(name)).SingleOrDefault() == null)
				{
					theme = new Theme()
					{
						ItemType = ItemType.Theme,
						Name = name,
						CreationDate = DateTime.Now,
						LastUpdatedInfo = DateTime.Now,
						NumberOfFollowers = 0,
						TrendingPercentage = 0.0,
						Baseline = 0.0,
						Informations = new List<Information>(),
						SubPlatform = subPlatform,
						Keywords = keywords
					};
					itemRepo.CreateItem(theme);
					uowManager.Save();
					newThemes.Add(theme);
				}
			}

			//Generate default widgets for items
			foreach (Item item in newThemes) GenerateDefaultItemWidgets(item.Name, item.ItemId);
			uowManager = null;

			return true;
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
						NumberOfFollowers = 0,
						TrendingPercentage = 0.0,
						Baseline = 10.0,
						Informations = new List<Information>(),
						SocialMediaUrls = new List<SocialMediaName>(),
						SubPlatform = subPlatform
					};
					itemRepo.CreateItem(organisation);
					uowManager.Save();
					organisations.Add(organisation);
				}
			}

			//Generate default widgets for items
			foreach (Item item in organisations) GenerateDefaultItemWidgets(item.Name, item.ItemId);
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

					Person person = (Person)AddItem(itemType: ItemType.Person, name: fullname, gender: personGender, district: district,
						level: level, site: site, position: position, dateOfBirth: dateOfBirth);
					person.SubPlatform = subPlatform; person.Area = areas.Where(x => x.PostalCode.Equals(postalCode) && x.Residence.ToLower().Equals(town.ToLower())).SingleOrDefault();

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
					person.Organisation = (Organisation)organisations.Where(x => x.Name.Equals(organisation)).SingleOrDefault();

					items.Add(person);
				}
			}

			if (items.Count > 0)
			{
				itemRepo.CreateItems(items);
				uowManager.Save();
				foreach (Item item in items) GenerateDefaultItemWidgets(item.Name, item.ItemId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Fills all items with recent data from the last
		/// synchronisation
		/// </summary>
		public void FillItems()
		{
			DataManager dataManager = new DataManager();
			IEnumerable<Item> items = GetAllItems();

			//Update sentiment & number of mentions
			foreach (Item item in items)
			{
				//Number of mentions from the last 30 days
				item.NumberOfMentions = dataManager.GetInformationsForItemid(item.ItemId).Where(info => info.CreationDate >= DateTime.Now.AddDays(-30)).Count();

				//Gather sentiment
				double sentiment = 0.0;
				int counter = 0;
				IEnumerable<Information> informations = dataManager.GetInformationsWithAllInfoForItem(item.ItemId)
																	 .Where(info => info.CreationDate >= DateTime.Now.AddMonths(-1))
																	 .AsEnumerable();
				foreach (Information info in informations)
				{
					foreach (PropertyValue propvalue in info.PropertieValues)
					{
						if (propvalue.Property.Name.ToLower().Equals("sentiment"))
						{
							double propSen = +Double.Parse(propvalue.Value);
							if (propSen != 0) sentiment += propSen / 100;
							counter++;
						}
					}
				}

				//Determine sentiment
				if (sentiment != 0)
				{
					sentiment = Math.Round((sentiment / counter), 2);
					if (sentiment >= 0) item.SentimentPositve = sentiment;
					else item.SentimentNegative = Math.Abs(sentiment);
				}
			}

			//Persist changes
			ChangeItems(items);

			//Determine trending
			foreach (Item item in items) DetermineTrending(item.ItemId);
		}

		/// <summary>
		/// Changes all the the given items
		/// </summary>
		public IEnumerable<Item> ChangeItems(IEnumerable<Item> items)
		{
			InitRepo();
			itemRepo.UpdateItems(items);
			return items;
		}

		/// <summary>
		/// Removes all given items from the database
		/// </summary>
		public void RemoveOverflowingItems()
		{
			InitRepo();
			IEnumerable<Item> itemsToRemove = GetAllItems().Where(item => item.SubPlatform == null).AsEnumerable();
			itemRepo.DeleteItems(itemsToRemove);
		}

		/// <summary>
		/// Changes profile picture of given user.
		/// </summary>
		public Item ChangePicture(int itemId, HttpPostedFileBase poImgFile)
		{
			InitRepo();

			//Get User
			Item itemToUpdate = itemRepo.ReadItem(itemId);
			if (itemToUpdate == null) return null;

			//Change profile picture
			byte[] imageData = null;
			using (var binary = new BinaryReader(poImgFile.InputStream))
			{
				imageData = binary.ReadBytes(poImgFile.ContentLength);
			}
			itemToUpdate.Picture = imageData;

			//Update database
			itemRepo.UpdateItem(itemToUpdate);
			return itemToUpdate;

		}

		/// <summary>
		/// Gives back all the items with all the informations
		/// </summary>
		public IEnumerable<Person> GetAllItemsWithInformations()
		{
			InitRepo();
			return itemRepo.ReadAllItemsWithInformations().AsEnumerable();
		}
	}
}
