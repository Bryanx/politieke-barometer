using BAR.BL.Domain.Data;
using BAR.DAL.EF;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of
	/// information. A single information object could be:
	/// - A tweet
	/// - A facebook post
	/// - etc.
	/// </summary>
	public class DataRepository : IDataRepository
	{
		private BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public DataRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Creates a new instance of an information objects and
		/// persists it to the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateInformations(IEnumerable<Information> infos)
		{
			ctx.Configuration.AutoDetectChangesEnabled = false;
			ctx.Informations.AddRange(infos);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Returns a list of informations based on
		/// a specific item.
		/// </summary>
		public IEnumerable<Information> ReadInformationsForItemid(int itemId)
		{
			return ctx.Informations.Include(info => info.Items)
								   .Where(info => info.Items.Any(item => item.ItemId == itemId))
								   .AsEnumerable();
		}

		/// <summary>
		/// Gives back a scource by sourcename
		/// </summary>
		public Source ReadSource(string sourceName)
		{
			return ctx.Sources.Where(src => src.Name.ToLower().Equals(sourceName.ToLower()))
							  .SingleOrDefault();
		}

		/// <summary>
		/// Creates a source and persists that to the database
		/// </summary
		public int CreateSource(Source source)
		{
			ctx.Sources.Add(source);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back a source
		/// </summary>
		public Source ReadSource(int sourceId)
		{
			return ctx.Sources.Find(sourceId);
		}

		/// <summary>
		/// Removes a source from the database
		/// </summary>
		public int DeleteSource(Source source)
		{
			ctx.Sources.Remove(source);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Returns the first audit from the database
		/// </summary>
		public SynchronizeAudit ReadLastAudit()
		{
			return ctx.SynchronizeAudits.Where(audit => audit.Succes)
										.OrderByDescending(audit => audit.TimeStamp)
										.FirstOrDefault();
		}

		/// <summary>
		/// Creates an audit in the database
		/// </summary>
		public int CreateAudit(SynchronizeAudit audit)
		{
			ctx.SynchronizeAudits.Add(audit);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back an audit
		/// </summary>
		public SynchronizeAudit ReadAudit(int auditId)
		{
			return ctx.SynchronizeAudits.Find(auditId);
		}

		/// <summary>
		/// Updates an audit and persists that to the database
		/// </summary>
		public int UpdateAudit(SynchronizeAudit audit)
		{
			ctx.Entry(audit).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back all the properties
		/// 
		/// NOTE
		/// ToList is needed to prevent crash issue with multiple datareaders
		/// </summary>
		public List<Property> ReadAllProperties()
		{
			return ctx.Properties.ToList();
		}

		/// <summary>
		/// Gives back all the sources
		/// 
		/// NOTE
		/// ToList is needed to prevent crash issue with multiple datareaders
		/// </summary>
		public List<Source> ReadAllSources()
		{
			return ctx.Sources.ToList();
		}

		/// <summary>
		/// Returns a list of all the informations objects that are
		/// related to a specific item.
		/// </summary>
		public IEnumerable<Information> ReadInformationsWithAllInfoForItem(int itemId)
		{
			return ctx.Informations.Include(info => info.Items)
								   .Include(info => info.PropertieValues)
								   .Include(info => info.PropertieValues.Select(propval => propval.Property))
								   .Where(info => info.Items.Any(item => item.ItemId == itemId))
								   .AsEnumerable();
		}

		/// <summary>
		/// Gives back all datasources
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DataSource> ReadAllDataSources()
		{
			return ctx.DataSources.AsEnumerable();
		}

		/// <summary>
		/// Gives back the datasources based on thd datasource ID.
		/// </summary>
		public DataSource ReadDataSource(int dataSourceId)
		{
			return ctx.DataSources.Find(dataSourceId);
		}

		/// <summary>
		/// Deletes the given datasource in the database
		/// </summary>
		public int DeleteDataSource(DataSource dataSource)
		{
			ctx.DataSources.Remove(dataSource);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a datasource in the database
		/// </summary>
		public int UpdateDataSource(DataSource dataSource)
		{
			ctx.Entry(dataSource).State = EntityState.Modified;
			return ctx.SaveChanges();
		}
	}
}


