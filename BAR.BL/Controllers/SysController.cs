using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using System;
using System.Collections.Generic;

namespace BAR.BL.Controllers
{
	/// <summary>
	/// Controller used for backgroundtasks.
	/// </summary>
	public class SysController
	{
		/// <summary>
		/// When new data arrives this method will be triggerd to determine
		/// a new baseline and a new trending percentage for a specific item.
		/// </summary>
		public void DetermineTrending(int itemId)
		{
			ItemManager itemManager = new ItemManager();
			itemManager.DetermineTrending(itemId);
		}

		public void DetermineTrending()
		{
			ItemManager itemManager = new ItemManager();
			IEnumerable<Item> allItems = itemManager.getAllItems();

			foreach(Item item in allItems)
			{
				DetermineTrending(item.ItemId);
			}
		}

		/// <summary>
		/// When a new baseline and trendingpercentage is determined,
		/// Alerts will be generated.
		/// </summary>
		public void GenerateAlerts(int itemId)
		{
			SubscriptionManager subManager = new SubscriptionManager();
			subManager.GenerateAlerts(itemId);
		}

		public void GenerateAlerts()
		{
			ItemManager itemManager = new ItemManager();
			IEnumerable<Item> allItems = itemManager.getAllItems();
			
			foreach (Item item in allItems)
			{
				GenerateAlerts(item.ItemId);
			}
		}
	}
}
