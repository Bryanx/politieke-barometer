using System;
using BAR.DAL;
using BAR.BL.Domain.Items;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using System.Linq;

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
			IEnumerable<Information> allInfoForId = dataManager.getAllInformationForId(itemId);

			DateTime earliestInfoDate = allInfoForId.Min(item => item.CreatetionDate).Value;
			DateTime lastInfoDate = allInfoForId.Max(item => item.CreatetionDate).Value;

			int period = (lastInfoDate - earliestInfoDate).Days;

			Console.WriteLine(earliestInfoDate);
			Console.WriteLine(lastInfoDate);

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
		/// Returns an item from a specific itemId.
		/// </summary>
		public Item GetItem(int itemId)
		{
			InitRepo();
			return itemRepo.ReadItem(itemId);
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
		/// Returns a list of all items.
		/// </summary>
		public IEnumerable<Item> getAllItems()
		{
			InitRepo();
			return itemRepo.ReadAllItems();
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
						ItemType = itemType,
						Name = name,
						CreationDate = DateTime.Now,
						LastUpdated = DateTime.Now,
						Description = description,
						NumberOfFollowers = 0,
						TrendingPercentage = 0.0,
						Baseline = 0.0,
						Informations = new List<Information>(),
						SocialMediaUrls = new List<SocialMediaUrl>(),
						Function = function
					};
					break;
				case ItemType.Organisation:
					item = new Organisation()
					{
						ItemType = itemType,
						Name = name,
						CreationDate = DateTime.Now,
						LastUpdated = DateTime.Now,
						Description = description,
						NumberOfFollowers = 0,
						TrendingPercentage = 0.0,
						Baseline = 0.0,
						Informations = new List<Information>(),
						SocialMediaUrls = new List<SocialMediaUrl>()
					};
					break;
				case ItemType.Theme:
					item = new Theme()
					{
						ItemType = itemType,
						Name = name,
						CreationDate = DateTime.Now,
						LastUpdated = DateTime.Now,
						Description = description,
						NumberOfFollowers = 0,
						TrendingPercentage = 0.0,
						Baseline = 0.0,
						Informations = new List<Information>(),
						Category = category
					};
					break;
				default:
					item = null;
					break;
			}

			if (item == null) return item;
			else
			{
				itemRepo.CreateItem(item);
				return item;
			}

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
