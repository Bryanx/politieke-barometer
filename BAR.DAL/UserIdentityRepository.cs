using BAR.BL.Domain.Users;
using BAR.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	/// <summary>
	/// TODO
	/// </summary>
	public class UserIdentityRepository : UserStore<User>
	{
		/// <summary>
		/// TODO
		/// </summary>
		public UserIdentityRepository(BarometerDbContext ctx) : base(ctx) { }
	}
}
