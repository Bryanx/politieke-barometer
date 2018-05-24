using BAR.BL.Domain.Items;
using BAR.DAL.EF;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of
	/// items. An item object could be:
	/// - A person
	/// - An organisation
	/// - A theme
	/// </summary>
	public class ItemRepository : IItemRepository
	{
		private readonly BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public ItemRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Item ReadItem(int itemId)
		{
			return ctx.Items.Find(itemId);
		}

		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Person ReadPersonWithDetails(int itemId)
		{
			return ctx.Items.OfType<Person>()
							.Include(item => item.SubPlatform)
							.Include(item => item.Area)
							.Include(item => item.Organisation)
							.Include(item => item.SocialMediaNames)
							.Include(item => item.SocialMediaNames.Select(social => social.Source))
							.Where(item => item.ItemId == itemId && !item.Deleted)
							.SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Organisation ReadOrganisationWithDetails(int itemId)
		{
			return ctx.Items.OfType<Organisation>()
							.Include(item => item.SocialMediaUrls)
							.Include(item => item.SocialMediaUrls.Select(social => social.Source))
							.Where(item => item.ItemId == itemId && !item.Deleted)
							.SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matchkes the itemId.
		/// </summary>
		public Theme ReadThemeWithDetails(int itemId)
		{
			return ctx.Items.OfType<Theme>()
							.Include(item => item.Keywords)
							.Where(item => item.ItemId == itemId && !item.Deleted)
							.SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Item ReadItemWithWidgets(int itemId)
		{
			return ctx.Items.Include(item => item.ItemWidgets)
							.Where(item => item.ItemId == itemId)
							.SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matches the itemId including SubPlatform
		/// </summary>
		public Item ReadItemWithPlatform(int itemId)
		{
			return ctx.Items.Include(item => item.SubPlatform)
							.Where(item => item.ItemId == itemId)
							.SingleOrDefault();
		}

		/// <summary>
		/// Returns a list of all items.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Item> ReadAllItemsWithPlatforms()
		{
			return ctx.Items.Include(item => item.ItemWidgets)
							.Include(item => item.SubPlatform)
							.AsEnumerable();
		}

		/// <summary>
		/// Returns a list of all persons.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Person> ReadAllPersonsWithPlatforms()
		{
			return ctx.Items.OfType<Person>()
							  .Include(item => item.Area)
							  .Include(item => item.ItemWidgets)
							  .Include(item => item.SubPlatform)
							  .Include(item => item.Organisation)
							  .Include(item => item.SocialMediaNames)
							  .Include(item => item.SocialMediaNames.Select(social => social.Source))
							  .AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the persons
		/// </summary>
		public IEnumerable<Person> ReadAllPersons()
		{
			return ReadAllPersonsWithPlatforms().AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the persons associated with a certain organisation
		/// </summary>
		public IEnumerable<Person> ReadAllPersonsForOrganisation(int organisationId)
		{
			return ReadAllItemsWithPlatforms().OfType<Person>()
											  .Where(item => item.Organisation.ItemId.Equals(organisationId))
											  .AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the organisations
		/// </summary>
		public IEnumerable<Organisation> ReadAllOraginsations()
		{
			return ReadAllItemsWithPlatforms().OfType<Organisation>().AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the themes
		/// </summary>
		public IEnumerable<Theme> ReadAllThemes()
		{
			return ctx.Items.OfType<Theme>()
							.Include(item => item.Keywords)
							.Include(item => item.ItemWidgets)
							.Include(item => item.SubPlatform)
							.AsEnumerable();
		}

		/// <summary>
		/// Persists an item to the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateItem(Item item)
		{
			ctx.Items.Add(item);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates an item and persists changes to the database
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateItem(Item item)
		{
			ctx.Entry(item).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a list of items and persists changes to the database
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateItems(IEnumerable<Item> items)
		{
			foreach (Item item in items) ctx.Entry(item).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a person and persists changes to the database
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdatePerson(Person person)
		{
			ctx.Entry(person).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes an item from the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteItem(Item item)
		{
			ctx.Items.Remove(item);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a list of items from the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteItems(IEnumerable<Item> items)
		{
			ctx.Items.RemoveRange(items);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Creates a range of items.
		/// </summary>
		public int CreateItems(IEnumerable<Item> items)
		{
			ctx.Items.AddRange(items);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Reads an organisation with a given name.
		/// </summary>
		public Organisation ReadOrganisation(string organisationName)
		{
			return ctx.Items.OfType<Organisation>()
							.Include(org => org.SubPlatform)
							.Where(org => org.Name.ToLower().Equals(organisationName.ToLower()))
							.SingleOrDefault();
		}

		/// <summary>
		/// Gives back all the items with all the informations
		/// </summary>
		public IEnumerable<Person> ReadAllItemsWithInformations()
		{
			return ctx.Items.OfType<Person>()
							.Include(item => item.Informations)
							.AsEnumerable();
		}

		/// <summary>
		/// Gives back all the items that are related to a
		/// specific organisation
		/// </summary>
		public IEnumerable<Item> ReadItemsForOrganisation(int itemId)
		{
			return ctx.Items.OfType<Person>()
							.Include(item => item.Organisation)
							.Include(item => item.Informations)
							.Where(item => item.Organisation.ItemId == itemId)
							.AsEnumerable();
		}
	}
}
