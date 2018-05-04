﻿using BAR.BL.Domain;
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
		Person GetPersonWithDetails(int itemId);
		Organisation GetOrganisationWithDetails(int itemId);
		Item GetItemWithSubPlatform(int itemId);
		Item GetItemWithAllWidgets(int itemId);
		IEnumerable<Item> GetAllItems();
		IEnumerable<Person> GetAllPersons();
		IEnumerable<Organisation> GetAllOrganisations();
		IEnumerable<Item> GetAllThemes();
		IEnumerable<Item> GetItemsForType(ItemType type);
		Person GetPerson(string personName);
		IEnumerable<Item> GetMostTrendingItems(int numberOfItems = 5);
		IEnumerable<Item> GetMostTrendingItemsForType(ItemType type, int numberOfItems = 5);
		IEnumerable<Item> GetMostTredningItemsForUser(string userId, int numberOfItems = 5);
		IEnumerable<Item> GetMostTredningItemsForUserAndItemType(string userId, ItemType type, int numberOfItems = 5);

		IEnumerable<Item> GetAllPersonsForSubplatform(int subPlatformID);
		IEnumerable<Item> GetAllOrganisationsForSubplatform(int subPlatformID);

		Item AddItem(ItemType itemType, string name, string description = "", string function = "", Category category = null,
			string district = null, string level = null, string site = null, Gender gender = Gender.OTHER, string position = null, DateTime? dateOfBirth = null);

		bool ImportJson(string json, int subPlatformID);
		bool ImportThemes(string json, int subPlatformID);
		
		Item ChangeItemName(int itemId, string name);
		Item ChangeItemActivity(int itemId);
		Person ChangePerson(int itemId, DateTime birthday, Gender gender, string position, string district);
		Item ChangePicture(int itemId, HttpPostedFileBase poImgFile);
    
		IEnumerable<Item> ChangeItems(IEnumerable<Item> items);
		void RemoveItem(int itemId);
		void RemoveOverflowingItems();
		void FillItems();
		void DetermineTrending(int itemId);
		double GetTrendingPer(int itemId);
    	string ConvertPfbToString(HttpPostedFileBase pfb);
  }
}
