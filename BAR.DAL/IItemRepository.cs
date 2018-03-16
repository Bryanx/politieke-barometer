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
		//...

		//Update
		void UpdateItemTrending(int itemId, double baseline, double trendingepr);
		void UpdateLastUpdated(int itemId, DateTime lastUpdated);		

		//Delete
		//..
	}
}
