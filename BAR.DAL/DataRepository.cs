using BAR.BL.Domain.Data;
using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Entity;
using BAR.BL.Domain.Items;

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
		/// Deletes a specific information object
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// All of the the propertyvalues of the information also need to be deleted.
		/// 
		/// NOTE
		/// Normally we don't delete informations.
		/// </summary>
		public int DeleteInformation(int infoId)
		{
			Information infoToDelete = ReadInformationWitlAllInfo(infoId);
			ctx.Informations.Remove(infoToDelete);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a range of information objects
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// All the propertyvalues of the informations also need the be deleted.
		/// 
		/// NOTE
		/// Normally we don't delete informations.
		/// </summary>
		public int DeleteInformations(IEnumerable<int> infoIds)
		{
			//Get informations to delete
			List<Information> infosToDelete = new List<Information>();
			foreach (int infoId in infoIds) infosToDelete.Add(ReadInformationWitlAllInfo(infoId));
			
			//Delete informations
			ctx.Informations.RemoveRange(infosToDelete);
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
		/// Gives back a list of information-objects.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Information> ReadAllInformations()
		{
			return ctx.Informations.AsEnumerable();
		}

		/// <summary>
		/// Gives back an information object based on informationid.
		/// </summary>
		public Information ReadInformation(int informationid)
		{
			return ctx.Informations.Find(informationid);
		}

		/// <summary>
		/// Gives back an information object with his property-values
		/// based on informationid.
		/// </summary>
		public Information ReadInformationWitlAllInfo(int informationId)
		{
			return ctx.Informations.Include(info => info.PropertieValues)
								   .Include(info => info.PropertieValues.Select(propval => propval.Property))
								   .Where(info => info.InformationId == informationId).SingleOrDefault();
		}

		/// <summary>
		/// Gets the number of informations of a specific given item
		/// from the creation date untill now.
		/// </summary
		public int ReadNumberInfo(int itemId, DateTime since)
		{
			return ctx.Informations
				.Where(info => info.Items.Any(item => item.ItemId == itemId))
				.Where(info => info.CreationDate >= since).Count();
		}

		/// <summary>
		/// Updates an instance of a specific information object
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateInformation(Information info)
		{
			ctx.Entry(info).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates all informations objects that are in the list.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateInformations(IEnumerable<Information> infos)
		{
			foreach (Information info in infos) ctx.Entry(info).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back a property by propertyname
		/// </summary>
		public Property ReadProperty(string propertyName)
		{
			return ctx.Properties.Where(prop => prop.Name.ToLower().Equals(propertyName.ToLower()))
								 .SingleOrDefault();
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
			return ctx.Informations//.Include(info => info.Items)
								   .Include(info => info.PropertieValues)
								   .Include(info => info.PropertieValues.Select(propval => propval.Property))
								   .Where(info => info.Items.Any(item => item.ItemId == itemId))
								   .AsEnumerable();
		}

        public IEnumerable<DataSource> ReadAllDataSources()
        {
            return ctx.DataSources.AsEnumerable();
        }

        public DataSource ReadDataSource(int dataSourceId)
        {
            return ctx.DataSources.Find(dataSourceId);
        }

        public int CreateDataSource(DataSource dataSource)
        {
            ctx.DataSources.Add(dataSource);
            return ctx.SaveChanges();
        }

        public int UpdateDataSource(DataSource dataSource)
        {
            ctx.Entry(dataSource).State = EntityState.Modified;
            return ctx.SaveChanges();
        }

        public int DeleteDataSource(DataSource dataSource)
        {
            ctx.DataSources.Remove(dataSource);
            return ctx.SaveChanges();
        }
        /// <summary>
		/// Returns the timer interval of the subplatform.
		/// </summary>
        public int ReadInterval(int dataSourceId)
        {
            return ctx.DataSources.Find(dataSourceId).Interval;
        }
        /// <summary>
		/// Returns the set time of the timer.
		/// </summary>
        public string ReadStartTime(int dataSourceId)
        {
            return ctx.DataSources.Find(dataSourceId).SetTime;
        }
        /// <summary>
		/// Updates the timer interval of the subplatform.
		/// </summary>
        public int UpdateInterval(int dataSourceId, int interval)
        {
            ctx.DataSources.Find(dataSourceId).Interval = interval;
            return ctx.SaveChanges();
        }
        /// <summary>
		/// Updates the set time of the timer.
		/// </summary>
        public int UpdateStartTime(int dataSourceId, string setTime)
        {
            ctx.DataSources.Find(dataSourceId).SetTime = setTime;
            return ctx.SaveChanges();
        }
    }
}


