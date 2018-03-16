﻿using System;
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

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public ItemRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}
		
		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Item ReadItem(int itemId)
		{
			return ctx.Items.Find(itemId);
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

		/// <summary>
		/// Returns a list of all items.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Item> ReadAllItems()
		{
			return ctx.Items.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of items that were
		/// last updated from now until a given date.
		/// </summary>
		public IEnumerable<Item> ReadAllItemsForUpdatedSince(DateTime since)
		{
			return ctx.Items.Where(item => item.LastUpdated >= since).AsEnumerable();
		}

		/// <summary>
		/// Persists an item to the database.
		/// Returns -1 if saveChanges() failed.
		/// </summary>
		public int CreateItem(Item item)
		{
			ctx.Items.Add(item);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates the Lastupdated field of an Item.
		/// Returns -1 if saveChanges() failed.
		/// </summary>
		public int UpdateLastUpdated(int itemId, DateTime lastUpdated)
		{
			Item itemToUpdate = ReadItemWithInformations(itemId);
			itemToUpdate.LastUpdated = lastUpdated;
			return UpdateItem(itemToUpdate);
		}

		/// <summary>
		/// Updates the baseline and trendingpercentage of a specific item.
		/// Returns -1 if saveChanges() failed.
		/// </summary>
		public int UpdateItemTrending(int itemId, double baseline, double trendingeper)
		{
			Item itemToUpdate = ReadItem(itemId);
			itemToUpdate.Baseline = baseline;
			itemToUpdate.TrendingPercentage = trendingeper;
			return UpdateItem(itemToUpdate);
		}

		/// <summary>
		/// Updates an item and persists changes to the database
		/// Returns -1 if saveChanges() failed.
		/// </summary>
		public int UpdateItem(Item item)
		{
			ctx.Entry(item).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a list of items and persists changes to the database
		/// Returns -1 if saveChanges() failed.
		/// </summary>
		public int UpdateItems(IEnumerable<Item> items)
		{
			foreach (Item item in items) ctx.Entry(item).State = EntityState.Modified;	
			return ctx.SaveChanges();
		}
	}
}
