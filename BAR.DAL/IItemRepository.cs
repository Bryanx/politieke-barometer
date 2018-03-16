using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	public interface IItemRepository
	{
		//Read
		Item ReadItemWithInformations(int itemId);
		Item ReadItem(int itemId);
		IEnumerable<Item> ReadAllItems();
		IEnumerable<Item> ReadAllItemsForUpdatedSince(DateTime since);

		//Create
		int CreateItem(Item item);

		//Update
		int UpdateItem(Item item);
		int UpdateItems(IEnumerable<Item> items);
		int UpdateItemTrending(int itemId, double baseline, double trendingepr);
		int UpdateLastUpdated(int itemId, DateTime lastUpdated);

		//Delete
		int DeleteItem(Item item);
		int DeleteItems(IEnumerable<Item> items);
	}
}
