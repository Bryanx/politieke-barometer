using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using BAR.DAL.EF;

namespace BAR.DAL
{
	public class UserRepository : IUserRepository
	{
		private BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public UserRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Returns all the users.
		/// </summary>
		public IEnumerable<User> ReadAllUsers()
		{
			return ctx.Users.AsEnumerable();
		}

<<<<<<< HEAD
    /// <summary>
    /// Returns the user form a specific userId.
    /// </summary>
    public User ReadUser(int userId)
    {
      var user = ctx.Users.Find(userId);
      return user;
    }
  }
=======
		/// <summary>
		/// Returns the user form a specific userId.
		/// </summary>
		public User ReadUser(int userId)
		{
			var user = ctx.Users.Find(userId);
			return user;
		}
	}
>>>>>>> 171b2c52f599e3140265097553dbe6f447cae59b
}
