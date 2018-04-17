using System;
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
		/// Reads a subplatform based on name of the subplatform
		/// </summary>
		/// <param name="subplatformName"></param>
		/// <returns></returns>
		public SubPlatform ReadSubPlatform(string subplatformName)
		{
			return ctx.SubPlatforms.Where(sp => sp.Name.Equals(subplatformName)).SingleOrDefault();
		}

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public SubplatformRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

	}
}
