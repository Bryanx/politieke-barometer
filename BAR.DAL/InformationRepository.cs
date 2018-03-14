using BAR.BL.Domain.Data;
using BAR.DAL.EF;
using Newtonsoft.Json;
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

		public IEnumerable<Information> ReadAllInfoForId(int itemId)
		{
			return ctx.Informations.Where(info => info.Item.ItemId == itemId).AsEnumerable();
		}

		/// <summary>
		/// Gets the number of informations of a specific given item
		/// form since till now.
		/// </summary
		public int ReadNumberInfo(int itemId, DateTime since)
		{
			return ctx.Informations.Where(info => info.Item.ItemId == itemId)
				.Where(info => info.LastUpdated >= since).Count();
		}
	}
}


