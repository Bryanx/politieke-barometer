using BAR.BL.Domain;
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
		IEnumerable<Item> GetAllItems();
		IEnumerable<Item> GetAllPersons();
		IEnumerable<Item> GetAllOrganisations();
		IEnumerable<Item> GetItemsForType(ItemType type);
		IEnumerable<Item> GetAllPersonsForSubplatform(int subPlatformID);


		Item CreateItem(ItemType itemType, string name, string description = "", string function = "", Category category = null);

		Item ChangeItemName(int itemId, string name);
		Item ChangeItemDescription(int itemId, string description);
		Item ChangeItemActivity(int itemId);

		void DetermineTrending(int itemId);
		double GetTrendingPer(int itemId);
	}
}
