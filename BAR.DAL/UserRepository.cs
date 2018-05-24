using BAR.BL.Domain.Users;
using System.Collections.Generic;
using System.Linq;
using BAR.DAL.EF;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of users.
	/// 
	/// NOTE
	/// This class has nothing to do with the identity-framework
	/// for this, we reference to the UserIdentityRepository.
	/// </summary>
	public class UserRepository : IUserRepository
	{
		private readonly BarometerDbContext ctx;

		/// <summary>
		/// Checks if manager will work with uow.
		/// </summary>
		public UserRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Returns a list of all users.
		/// </summary>
		public IEnumerable<User> ReadAllUsers()
		{
			return ctx.Users.AsEnumerable();
		}

		/// <summary>
		/// Returns the user with a specific userId.
		/// </summary>
		public User ReadUser(string userId)
		{
			return ctx.Users.Include(user => user.Area).Where(area => area.Id.Equals(userId))
							.SingleOrDefault();
		}

		/// <summary>
		/// Creates an instance of a user in the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateUser(User user)
		{
			ctx.Users.Add(user);
			return ctx.SaveChanges();
		}

		/// <summary>;
		/// Updates a given user
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateUser(User user)
		{
			ctx.Entry(user).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a given list of users.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateUsers(IEnumerable<User> users)
		{
			foreach (User user in users) ctx.Entry(user).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Reads all areas.
		/// </summary>
		public IEnumerable<Area> ReadAllAreas()
		{
			return ctx.Areas.AsEnumerable();
		}

		/// <summary>
		/// Reads selected area.
		/// </summary>
		public Area ReadArea(int areaId)
		{
			return ctx.Areas.Find(areaId);
		}

		/// <summary>
		/// Reads all roles.
		/// </summary>
		public IEnumerable<IdentityRole> ReadAllRoles()
		{
			return ctx.Roles.AsEnumerable();
		}

		/// <summary>
		/// Reads role of given user.
		/// </summary>
		public IdentityRole ReadRole(string userId)
		{
			return ctx.Roles.Where(role => role.Users.Any(user => user.UserId.Equals(userId)))
							.SingleOrDefault();
		}

		/// <summary>
		/// Gives back all the users with all the alerts
		/// </summary>
		public IEnumerable<User> ReadAllUsersWithAlerts()
		{
			return ctx.Users.Include(user => user.Alerts).AsEnumerable();
		}
	}
}
