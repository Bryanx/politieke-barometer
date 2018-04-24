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
    public int CreateInformations(List<Information> infos)
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
      Information infoToDelete = ReadInformationWithPropValues(infoId);
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
      foreach (int infoId in infoIds)
      {
        Information infoToDelete = ReadInformationWithPropValues(infoId);
        ctx.Informations.Remove(infoToDelete);
      }
      return ctx.SaveChanges();
    }

    /// <summary>
    /// Returns a list of informations based on
    /// a specific item.
    /// </summary>
    public IEnumerable<Information> ReadInformationsForItemid(int itemId)
    {
      return ctx.Informations.Include(x => x.Items)
          .Where(info => info.Items.Any(item => item.ItemId == itemId)).AsEnumerable();
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
    public Information ReadInformationWithPropValues(int informationId)
    {
      return ctx.Informations.Include(info => info.PropertieValues.Select(propval => propval.Property))
        .Where(info => info.InformationId == informationId).SingleOrDefault();
    }

    /// <summary>
    /// Gives back all informations objects form now until a given date
    /// for a specific item.
    /// </summary>
    public IEnumerable<Information> ReadInformationsForDate(int itemId, DateTime since)
    {
      return ctx.Informations
        .Where(info => info.Items.Any(item => item.ItemId == itemId))
        .Where(info => info.CreationDate >= since).AsEnumerable();
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

    public Property ReadProperty(string propertyName)
    {
      return ctx.Properties.Where(x => x.Name.Equals(propertyName)).SingleOrDefault();
    }

    public Source ReadSource(string sourceName)
    {
      return ctx.Sources.Where(x => x.Name.Equals(sourceName)).SingleOrDefault();
    }

    public SynchronizeAudit ReadLastAudit()
    {
      return ctx.SynchronizeAudits.Where(x => x.Succes).OrderByDescending(x => x.TimeStamp).FirstOrDefault();
    }

    public int CreateAudit(SynchronizeAudit synchronizeAudit)
    {
      ctx.SynchronizeAudits.Add(synchronizeAudit);
      return ctx.SaveChanges();
    }

    public SynchronizeAudit ReadAudit(int synchronizeAuditId)
    {
      return ctx.SynchronizeAudits.Where(x => x.SynchronizeAuditId == synchronizeAuditId).SingleOrDefault();
    }

    public int UpdateAudit(SynchronizeAudit synchronizeAudit)
    {
      ctx.Entry(synchronizeAudit).State = EntityState.Modified;
      return ctx.SaveChanges();
    }

    public IEnumerable<Property> ReadAllProperties()
    {
      return ctx.Properties.ToList();
    }

    public IEnumerable<Source> ReadAllSources()
    {
      return ctx.Sources.ToList();
    }

    public int CreateSource(Source source)
    {
      ctx.Sources.Add(source);
      return ctx.SaveChanges();
    }

    public Source ReadSource(int sourceId)
    {
      return ctx.Sources.Where(x => x.SourceId == sourceId).SingleOrDefault();
    }

    public int DeleteSource(Source source)
    {
      ctx.Sources.Remove(source);
      return ctx.SaveChanges();
    }

    /// <summary>
		/// Returns a list of all the informations objects that are
		/// related to a specific item.
		/// </summary>
		public IEnumerable<Information> ReadInformationsWithAllInfoForItem(int itemId)
    {
      return ctx.Informations.Include(infoIncl => infoIncl.PropertieValues
                   .Select(propval => propval.Property))
                   .Where(info => info.Items.Any(item => item.ItemId == itemId)).AsEnumerable();
    }
  }
}


