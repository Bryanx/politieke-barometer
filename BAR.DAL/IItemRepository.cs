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
		Item ReadItem(int itemId);
		Item ReadItemWithInformations(int itemId);
		Item ReadItemWithPlatform(int itemId);
		Item ReadItemWithWidgets(int itemId);

		IEnumerable<Item> ReadAllItemsWithPlatforms();
		IEnumerable<Item> ReadAllItemsForUpdatedSince(DateTime since);
		
		Person ReadPerson(string personName);
		Person ReadPersonWithDetails(int itemId);
		
		Theme ReadThemeWithDetails(int itemId);
		
		IEnumerable<Person> ReadAllPersonsForOrganisation(int organisationId);
		
		Organisation ReadOrganisationWithDetails(int itemId);	
    	Organisation ReadOrganisation(string organisationName);
		
		IEnumerable<Organisation> ReadAllOraginsations();
		IEnumerable<Person> ReadAllPersonsWithPlatforms();	
		IEnumerable<Theme> ReadAllThemes();
		
		//Create
		int CreateItem(Item item);
    int CreateItems(IEnumerable<Item> items);

		//Update
		int UpdateItem(Item item);
		int UpdateItems(IEnumerable<Item> items);

		int UpdatePerson(Person person);

		//Delete
		int DeleteItem(Item item);
		int DeleteItems(IEnumerable<Item> items);
	}
}
