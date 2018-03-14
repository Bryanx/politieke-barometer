﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.DAL.EF;
using BAR.BL.Domain.Items;
using System.Data.Entity;

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
            return ctx.Items.Where(i => i.ItemId == itemId).SingleOrDefault();
        }

        /// <summary>
        /// Updates the Lastupdated field of an Item.
        /// </summary>
        public void UpdateLastUpdated(int itemId, DateTime LastUpdated)
        {
            //needs to be implemented in the informationRepository
            //because its the information in the items wich are updated
            //and not the item itself.
            throw new NotImplementedException();
        }
    }
}
