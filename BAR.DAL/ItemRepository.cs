using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.DAL.EF;
using BAR.BL.Domain.Items;
using System.Data.Entity;
using BAR.BL.Domain.Data;

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
				.Include(i => i.Area)
				.Include(i => i.Organisation)
				.Include(i => i.SocialMediaNames)
				.Include(i => i.SocialMediaNames.Select(s => s.Source))
				.Where(i => i.ItemId == itemId && i.Deleted == false)
				.SingleOrDefault();
		}
		
		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Organisation ReadOrganisationWithDetails(int itemId)
		{
			return ctx.Items.OfType<Organisation>()
				.Include(i => i.SocialMediaUrls)
				.Include(i => i.SocialMediaUrls.Select(s => s.Source))
				.Where(i => i.ItemId == itemId && i.Deleted == false)
				.SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matchkes the itemId.
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public Theme ReadThemeWithDetails(int itemId)
		{
			return ctx.Items.OfType<Theme>()
				.Include(i => i.Keywords)
				.Where(i => i.ItemId == itemId && i.Deleted == false)
				.SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matches the itemId.
		/// </summary>       
		public Item ReadItemWithWidgets(int itemId)
		{
			return ctx.Items.Include(item => item.ItemWidgets)
				.Where(item => item.ItemId == itemId).SingleOrDefault();
		}

		/// <summary>
		/// Returns the item that matches the itemId including SubPlatform
		/// </summary>
		public Item ReadItemWithSubPlatform(int itemId)
		{
			return ctx.Items
				.Include(i => i.SubPlatform)
				.Where(item => item.ItemId == itemId)
				.SingleOrDefault();
		}

		/// <summary>
		/// Gives back a list of all the items for a specific type
		/// </summary>
		public IEnumerable<Item> ReadItemsForType(ItemType type)
		{
			return ctx.Items.Where(item => item.ItemType == type).AsEnumerable();
		}

		/// <summary>
		/// Does the same thing as ReadItem but it loads all the
		/// informations with it.
		/// </summary>
		public Item ReadItemWithInformations(int itemId)
		{
			return ctx.Items.Include(item => item.Informations)
				.Where(item => item.ItemId == itemId).SingleOrDefault();
		}

		/// <summary>
		/// Returns a list of all items.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Item> ReadAllItems()
		{
			return ctx.Items.Include(item => item.ItemWidgets)
				            .Include(item => item.SubPlatform)
										.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the persons
		/// </summary>
		public IEnumerable<Person> ReadAllPersons()
		{
			return ReadAllItems().OfType<Person>().AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the persons associated with a certain organisation
		/// </summary>
		/// <param name="organisationId"></param>
		/// <returns></returns>
		public IEnumerable<Person> ReadAllPersonsForOrganisation(int organisationId)
		{
			return ReadAllItems().OfType<Person>()
				.Where(item => item.Organisation.ItemId.Equals(organisationId)).AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the organisations
		/// </summary>
		public IEnumerable<Organisation> ReadAllOraginsations()
		{
			return ReadAllItems().OfType<Organisation>().AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the themes
		/// </summary>
		public IEnumerable<Theme> ReadAllThemes()
		{
			return ctx.Items
				.OfType<Theme>().Include(item => item.Keywords)
				.Include(item => item.ItemWidgets)
				.Include(item => item.SubPlatform)
				.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of items that were
		/// last updated from a given date untill now.
		/// </summary>
		public IEnumerable<Item> ReadAllItemsForUpdatedSince(DateTime since)
		{
			return ctx.Items.Where(item => item.LastUpdatedInfo >= since).AsEnumerable();
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
		/// Updates the Lastupdated field of an Item.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateLastUpdated(int itemId, DateTime lastUpdated)
		{
			Item itemToUpdate = ReadItemWithInformations(itemId);
			itemToUpdate.LastUpdatedInfo = lastUpdated;
			return UpdateItem(itemToUpdate);
		}

		/// <summary>
		/// Updates the baseline and trendingpercentage of a specific item.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateItemTrending(int itemId, double baseline, double trendingeper)
		{
			Item itemToUpdate = ReadItem(itemId);
			itemToUpdate.Baseline = baseline;
			itemToUpdate.TrendingPercentage = trendingeper;
			return UpdateItem(itemToUpdate);
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
			foreach (Item item in items) ctx.Items.Remove(item);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Reads a person of a given name.
		/// </summary>
		public Person ReadPerson(string personName)
		{
			
			return ctx.Items.OfType<Person>().Where(i => i.Name.Equals(personName)).SingleOrDefault();
		}

		/// <summary>
		/// Creates a range of items.
		/// </summary>
		public int CreateItems(ICollection<Item> items)
		{
			ctx.Items.AddRange(items);
			return ctx.SaveChanges();
		}

        /// <summary>
        /// Reads an organisation with a given name.
        /// </summary>
        public Item ReadOrganisation(string organisationName)
        {
          return ctx.Items.Include(org => org.SubPlatform)
            .Where(x => x.Name.Equals(organisationName)).SingleOrDefault();
        }
	}
}
