using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using BAR.DAL.EF;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;

namespace BAR.DAL
{
	public class UserRepository : UserStore<User>, IUserRepository
	{
		private BarometerDbContext ctx;

		/// <summary>
		/// This constructor is used if
		/// you plan to not work with identity.
		/// 
		/// WARNING
		/// Methods that are being used with identity will not
		/// work if you plan to use this constructor
		/// </summary>
		public UserRepository(BarometerDbContext ctx, UnitOfWork uow = null) : base(ctx)
		{
			if (uow == null) this.ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// This constructor is used if
		/// you plan to not work with identity.
		/// 
		/// WARNING
		/// Methods that are being used with identity will not
		/// work if you plan to use this constructor
		/// </summary>
		public UserRepository(UnitOfWork uow = null)
		{
			if (uow == null) this.ctx = new BarometerDbContext();
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
		/// Gives back a list of users from a specific area.
		/// </summary>
		public IEnumerable<User> ReadAllUsersForArea(int areaId)
		{
			return ctx.Users.Where(user => user.Area.AreaId == areaId).AsEnumerable();
		}

		/// <summary>
		/// Returns a list of users for a specific role
		/// </summary>
		public IEnumerable<User> ReadAllUsersForRole(string roleId)
		{
      return ctx.Users.Where(x => x.Roles.Any(y => y.RoleId.Equals(roleId))).AsEnumerable();
    }

		/// <summary>
		/// Returns the user from a specific userId.
		/// </summary>
		public User ReadUser(string userId)
		{
      return ctx.Users.Include(x => x.Area).Where(x => x.Id.Equals(userId)).SingleOrDefault();
		}

		/// <summary>
		/// Returns a user with their activities.
		/// </summary>
		public User ReadUserWithActivities(string userId)
		{
			return ctx.Users.Include(user => user.Activities)
				.Where(user => user.Id.Equals(userId)).SingleOrDefault();
		}

		/// <summary>
		/// Reads all the activities of all users.
		/// </summary>
		public IEnumerable<Activity> ReadAllActivities()
		{
			List<Activity> activities = new List<Activity>();
			IEnumerable<User> userActivities = ctx.Users.Include(user => user.Activities).AsEnumerable();

			foreach (User user in userActivities) activities.AddRange(user.Activities);
			return activities.AsEnumerable();
		}

		/// <summary>
		/// Reads all the activities for a specific user.
		/// </summary>
		public IEnumerable<Activity> ReadActivitiesForUser(string userId)
		{
			return ctx.Users.Include(user => user.Activities).
				Where(user => user.Id.Equals(userId)).SingleOrDefault().Activities.AsEnumerable();
		}

		/// <summary>
		/// Creates an instance of a user in the database
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateUser(User user)
		{
			ctx.Users.Add(user);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a given user
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateUser(User user)
		{
			ctx.Entry(user).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updataes a given list of users
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateUsers(IEnumerable<User> users)
		{
			foreach (User user in users) ctx.Entry(user).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a given user from the database
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// if user is deleted, all the acitivies of a specific user also need to be deleted.
		/// Dashboard of the user also needs to be deleted.
		/// 
		/// NOTE
		/// Normally we don't delete a user, we disable his account but keep the information.
		/// </summary>
		public int DeleteUser(string userId)
		{
			User userToDelete = ReadUserWithActivities(userId);
			ctx.Users.Remove(userToDelete);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a given list of users
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// if users are deleted, all the acitivies of the users also need to be deleted.
		/// Dashboard of the users also needs to be deleted.
		/// 
		/// NOTE
		/// Normally we don't delete a users, we disable his account but keep the information
		/// </summary>
		public int DeleteUsers(IEnumerable<string> userIds)
		{
			foreach (string userid in userIds)
			{
				User userToDelete = ReadUserWithActivities(userid);
				ctx.Users.Remove(userToDelete);
			}
			return ctx.SaveChanges();
		}

    /// <summary>
    /// Reads all areas.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Area> ReadAreas()
    {
      return ctx.Areas;
    }

    /// <summary>
    /// Reads selected area.
    /// </summary>
    /// <param name="areaId"></param>
    /// <returns></returns>
    public Area ReadArea(int areaId)
    {
      return ctx.Areas.Where(x => x.AreaId == areaId).SingleOrDefault();
    }
  }
}
