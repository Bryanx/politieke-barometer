using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IItemManager
	{	
		Item GetItem(int itemId);
		IEnumerable<Item> getAllItems();
		void DetermineTrending(int itemId);
		double GetTrendingPer(int itemId);

		Item CreateItem(ItemType itemType, string name, string description = "", string function = "", Category category = null);
	}
}
