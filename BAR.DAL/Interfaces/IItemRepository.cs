using BAR.BL.Domain.Items;
using System.Collections.Generic;

namespace BAR.DAL
{
	public interface IItemRepository
	{
		//Read
		Item ReadItem(int itemId);
		Item ReadItemWithPlatform(int itemId);
		Item ReadItemWithWidgets(int itemId);
		IEnumerable<Item> ReadAllItemsWithPlatforms();
		IEnumerable<Item> ReadItemsForOrganisation(int itemId);
		Person ReadPersonWithDetails(int itemId);
		IEnumerable<Person> ReadAllItemsWithInformations();
		IEnumerable<Person> ReadAllPersonsWithPlatforms();
		IEnumerable<Person> ReadAllPersonsForOrganisation(int organisationId);
		Organisation ReadOrganisationWithDetails(int itemId);
		Organisation ReadOrganisation(string organisationName);
		IEnumerable<Organisation> ReadAllOraginsations();
		Theme ReadThemeWithDetails(int itemId);
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
