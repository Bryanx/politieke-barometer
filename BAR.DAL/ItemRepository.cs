using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.DAL.EF;
using BAR.BL.Domain.Items;
using System.Data.Entity;
using BAR.BL.Domain.Data;

namespace BAR.DAL
{
	/// <summary>
	/// At this moment the repository works HC.
	/// </summary>
	public class ItemRepository : IItemRepository
	{
		private BarometerDbContext ctx;

		public ItemRepository()
		{
			ctx = new BarometerDbContext();           
		}

		/// <summary>
		/// Updates the baseline and trendingpercentage of a specific item.       
		/// </summary>
		public void UpdateItemTrending(int itemId, double baseline, double trendingeper)
		{
			Item itemToUpdate = ReadItem(itemId);
			itemToUpdate.Baseline = baseline;
			itemToUpdate.TrendingPercentage = trendingeper;

			ctx.Entry(itemToUpdate).State = System.Data.Entity.EntityState.Modified;
			ctx.SaveChanges();
		}

		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Item ReadItem(int itemId)
		{
			return ctx.Items.Find(itemId);
		}

		/// <summary>
		/// Updates the Lastupdated field of an Item.
		/// </summary>
		public void UpdateLastUpdated(int itemId, DateTime lastUpdated)
		{
			Item itemToUpdate = ReadItemWithInformations(itemId);
			foreach (Information info in itemToUpdate.Informations) info.LastUpdated = lastUpdated;

			ctx.Entry(itemToUpdate).State = EntityState.Modified;
			ctx.SaveChanges();            
		}

		/// <summary>
		/// Does the same thing as ReadItem but it loeds all the
		/// informations with it.
		/// </summary>
		public Item ReadItemWithInformations(int itemId)
		{
			return ctx.Items.Include(item => item.Informations)
				.Where(item => item.ItemId == itemId).SingleOrDefault();
		}
	}
}
