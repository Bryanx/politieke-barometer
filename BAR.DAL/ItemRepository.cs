using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Items;

namespace BAR.DAL
{
    /// <summary>
    /// At this moment the repository works HC.
    /// </summary>
    public class ItemRepository : IItemRepository
    {
        private List<Item> items;

        public ItemRepository()
        {
            items = new List<Item>();
        }

        /// <summary>
        /// Updates the baseline and trendingpercentage of a specific item.       
        /// </summary>
        public void UpdateItemTrending(int itemId, double baseline, double trendingepr)
        {
            // all data lives in memory
            // everything refers to the same objects
        }

        /// <summary>
        /// Returns the item that matches the itemId.
        /// </summary>       
        public Item ReadItem(int itemId)
        {
            return items.Where(i => i.ItemId == itemId).SingleOrDefault();
        }

        /// <summary>
        /// Updates the Lastupdated field of an Item.
        /// </summary>
        public void UpdateLastUpdated(int ItemId, DateTime LastUpdated)
        {
            // all data lives in memory
            // everything refers to the same objects
        }
    }
}
