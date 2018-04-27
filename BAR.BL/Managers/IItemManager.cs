using BAR.BL.Domain;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAR.BL.Managers
{
	public interface IItemManager
	{
		Item GetItem(int itemId);
		Item GetPersonWithDetails(int itemId);
		Item GetOrganisationWithDetails(int itemId);
		Item GetItemWithSubPlatform(int itemId);
		Item GetItemWithAllWidgets(int itemId);
		IEnumerable<Item> GetAllItems();
		IEnumerable<Item> GetAllPersons();
		IEnumerable<Item> GetAllOrganisations();
		IEnumerable<Item> GetAllThemes();
		IEnumerable<Item> GetItemsForType(ItemType type);
		Item GetPerson(string personName);
		IEnumerable<Item> GetMostTrendingItems(int numberOfItems = 5);
		IEnumerable<Item> GetMostTrendingItemsForType(ItemType type, int numberOfItems = 5);
		IEnumerable<Item> GetMostTredningItemsForUser(string userId, int numberOfItems = 5);
		IEnumerable<Item> GetMostTredningItemsForUserAndItemType(string userId, ItemType type, int numberOfItems = 5);

		IEnumerable<Item> GetAllPersonsForSubplatform(int subPlatformID);

		Item AddItem(ItemType itemType, string name, string description = "", string function = "", Category category = null,
			string district = null, string level = null, string site = null, Gender gender = Gender.OTHER, string position = null, DateTime? dateOfBirth = null);

		bool ImportJson(string json, int subPlatformID);


		Item ChangeItemName(int itemId, string name);
		Item ChangeItemActivity(int itemId);

		void DetermineTrending(int itemId);
		double GetTrendingPer(int itemId);
    	string ConvertPfbToString(HttpPostedFileBase pfb);
  }
}
