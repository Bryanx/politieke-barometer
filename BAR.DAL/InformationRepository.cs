using BAR.BL.Domain.Data;
using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
		/// persist that to the database.
		/// </summary>
		public void CreateInformation(Information info)
		{
			ctx.Informations.Add(info);
			ctx.SaveChanges();
		}

		/// <summary>
		/// Returns a list of informations based on
		/// a specific item.
		/// </summary>
		public IEnumerable<Information> ReadAllInfoForId(int itemId)
		{
			return ctx.Informations.Where(info => info.Item.ItemId == itemId).AsEnumerable();
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
		/// Gets the number of informations of a specific given item
		/// form since till now.
		/// </summary
		public int ReadNumberInfo(int itemId, DateTime since)
		{
			return ctx.Informations.Where(info => info.Item.ItemId == itemId)
				.Where(info => info.CreatetionDate >= since).Count();
		}
	}
}


