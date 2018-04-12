using BAR.BL.Domain.Data;
using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace BAR.DAL
{
	/// <summary>
	/// At this moment the repository works HC.
	/// </summary>
	public class InformationRepository : IInformationRepository
	{
		private BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public InformationRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Creates a new instance of an information objects and
		/// persists it to the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateInformation(Information info)
		{
			ctx.Informations.Add(info);
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
		public IEnumerable<Information> ReadAllInfoForId(int itemId)
		{
			return ctx.Informations
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
			return ctx.Informations.Include(info => info.PropertieValues)
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
				.Where(info => info.CreatetionDate >= since).AsEnumerable();
		}

		/// <summary>
		/// Gets the number of informations of a specific given item
		/// from the creation date untill now.
		/// </summary
		public int ReadNumberInfo(int itemId, DateTime since)
		{
			return ctx.Informations
				.Where(info => info.Items.Any(item => item.ItemId == itemId))
				.Where(info => info.CreatetionDate >= since).Count();
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
	}
}


