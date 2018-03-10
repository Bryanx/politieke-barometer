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
    public class ItemRepository
    {
        private List<Item> items;

        public ItemRepository()
        {
            items = new List<Item>();
        }

        public void UpdateItemTrending(int itemId, double baseline, double trendingepr)
        {
            // all data lives in memory
            // everything refers to the same objects
        }

        public Item ReadItem(int itemId)
        {         
            return items.Where(i => i.ItemId == itemId).SingleOrDefault();
        }
    }
}
