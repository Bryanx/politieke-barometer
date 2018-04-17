﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain;
using BAR.DAL.EF;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of
	/// subplatforms.
	/// </summary>
	public class SubplatformRepository : ISubplatformRepository
	{
		private readonly BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public SubplatformRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Creates a new instance of a given subplatform in the database
		/// </summary>
		public int CreateSubplatform(SubPlatform subPlatform)
		{
			ctx.SubPlatforms.Add(subPlatform);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Reads a subplatform based on name of the subplatform.
		/// </summary>
		public SubPlatform ReadSubPlatform(string subplatformName)
		{
			return ctx.SubPlatforms.Where(sp => sp.Name.Equals(subplatformName)).SingleOrDefault();
		}

		/// <summary>
		/// Gives back all the subplatforms that are on the system
		/// </summary>
		public IEnumerable<SubPlatform> ReadSubPlatform()
		{
			return ctx.SubPlatforms.AsEnumerable();
		}
	}
}
