using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Web;

namespace BAR.BL.Managers
{
	public interface IItemManager
	{
		//Items
		Item GetItem(int itemId);
		Item GetItemWithSubPlatform(int itemId);
		Item GetItemWithAllWidgets(int itemId);
		IEnumerable<Item> GetAllItems();
		IEnumerable<Item> GetItemsForType(ItemType type);
		IEnumerable<Item> GetMostTrendingItems(int subplatformId, int numberOfItems = 5, bool useWithOldData = false);
		IEnumerable<Item> GetMostTrendingItemsForType(int subplatformId, ItemType type, int numberOfItems = 5, bool useWithOldData = false);
		IEnumerable<Item> GetMostTrendingItemsForUser(int subplatformId, string userId, int numberOfItems = 5, bool useWithOldData = false);
		IEnumerable<Item> GetMostTrendingItemsForUserAndItemType(int subplatformId, string userId, ItemType type, int numberOfItems = 5, bool useWithOldData = false);
		IEnumerable<Item> GetItemsForOrganisation(int itemId);
		IEnumerable<Item> GetAllOrganisationsForSubplatform(int subPlatformID);

		Item AddItem(ItemType itemType, string name, string description = "", string function = "",
			string district = null, string level = null, string site = null, Gender gender = Gender.Other, string position = null,
			DateTime? dateOfBirth = null, List<Keyword> keywords = null);

		Item ChangeItemName(int itemId, string name);
		Item ChangeItemPlatform(int itemId, int subplatformId);
		Item ChangeItemActivity(int itemId);
		Item ChangePicture(int itemId, HttpPostedFileBase poImgFile);
		IEnumerable<Item> ChangeItems(IEnumerable<Item> items);

		void RemoveItem(int itemId);
		
		//Perons
		Person GetPersonWithDetails(int itemId);
		IEnumerable<Person> GetAllPersons();
		IEnumerable<Person> GetAllPersonsForOrganisation(int organisationId);
		IEnumerable<Person> GetAllItemsWithInformations();
		IEnumerable<Person> GetAllPersonsForSubplatform(int subPlatformID);

		Person ChangePersonSocialMedia(int personId, string twitter, string facebook);
		Person ChangePerson(int itemId, DateTime birthday, Gender gender, string position, string district);
		Person ChangePerson(int itemId, string site);
		Person ChangePersonOrganisation(int itemId, int organisationId);

		//Organisations
		Organisation GetOrganisationWithDetails(int itemId);
		IEnumerable<Organisation> GetAllOrganisations();

		Organisation ChangeOrganisation(int itemId, string site);

		void FillOrganisations();

		//Themes
		Theme GetThemeWithDetails(int itemId);
		IEnumerable<Theme> GetAllThemes();
		IEnumerable<Theme> GetAllThemesForSubplatform(int subplatformId);

		bool ImportThemes(string json, int subPlatformID);

		//Others
		void RemoveOverflowingItems();
		void FillPersonesAndThemes();
		void GenerateDefaultItemWidgets(string name, int itemId);
		bool ImportJson(string json, int subPlatformID);
		void DetermineTrending(int itemId);
		string ConvertPfbToString(HttpPostedFileBase pfb);
		void RefreshItemData(int platformId);
	}
}
