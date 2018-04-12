﻿using BAR.BL.Domain.Users;
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
	/// Class which will handle all CRUD related to Identity.
	/// </summary>
	public class UserIdentityRepository : UserStore<User>
	{
		/// <summary>
		/// Context is given to base class (UserStore).
		/// </summary>
		public UserIdentityRepository(BarometerDbContext ctx) : base(ctx) { }
	}
}
