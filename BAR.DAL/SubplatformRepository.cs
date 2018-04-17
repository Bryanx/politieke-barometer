using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain;
using BAR.DAL.EF;
using BAR.BL.Domain.Core;

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
		/// Deletes a subplatform.
		/// 
		/// NOTE
		/// When you delete a subplatform, all other information of
		/// that subplatform will also be deleted.
		/// </summary>
		public int DeleteSubplatform(SubPlatform subPlatform)
		{
			//*** WARNING ***//
			//This method does yet delete the whole platform.
			//It just deletes the instance of the subplatform
			ctx.SubPlatforms.Remove(subPlatform);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back the configuration of a specific subplatform
		/// </summary>
		public Customization ReadCustomization(string subplatformName)
		{
			SubPlatform subPlatform = ctx.SubPlatforms.Include(platform => platform.Customization)
									.Where(platform => platform.Name.ToLower().Equals(subplatformName.ToLower())).SingleOrDefault();
			return subPlatform.Customization;
		}

		/// <summary>;;
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

		/// <summary>
		/// Updates a single subplatform.
		/// </summary>
		public int UpdateSubplatform(SubPlatform subPlatform)
		{
			ctx.Entry(subPlatform).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a given list of subplatforms
		/// </summary>
		public int UpdateSubplatforms(IEnumerable<SubPlatform> subPlatforms)
		{
			foreach (SubPlatform platform in subPlatforms) ctx.Entry(platform).State = EntityState.Modified;
			return ctx.SaveChanges();
		}
	}
}
