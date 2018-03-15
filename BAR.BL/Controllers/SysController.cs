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
		IItemManager itemManager;
		ISubscriptionManager subManager;
		public SysController()
		{
			itemManager = new ItemManager();
			subManager = new SubscriptionManager();
		}

		/// <summary>
		/// When new data arrives this method will be triggerd to determine
		/// a new baseline and a new trending percentage for a specific item.
		/// </summary>
		public void DetermineTrending(int itemId)
		{		
			itemManager.DetermineTrending(itemId);
		}

		/// <summary>
		/// Is used to trigger the determination
		/// of adjusting the baseline and the trending percentage 
		/// of a specific item.
		/// </summary>
		public void DetermineTrending()
		{
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
			subManager.GenerateAlerts(itemId);
		}

		public void GenerateAlerts()
		{
			IEnumerable<Item> allItems = itemManager.getAllItems();
			
			foreach (Item item in allItems)
			{
				GenerateAlerts(item.ItemId);
			}
		}
	}
}
