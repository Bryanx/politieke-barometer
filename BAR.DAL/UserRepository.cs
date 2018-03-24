using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using BAR.DAL.EF;
using System.Data.Entity;

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
		/// Returns a list of users for a spefic role
		/// </summary>
		public IEnumerable<User> ReadAllUsersForRole(int roleId)
		{
			return ctx.Users.Where(user => user.Role.RoleId == roleId).AsEnumerable();
		}

		/// <summary>
		/// Returns the user form a specific userId.
		/// </summary>
		public User ReadUser(int userId)
		{
			var user = ctx.Users.Find(userId);
			return user;
		}

		/// <summary>
		/// Gives back a user with their activities.
		/// </summary>
		public User readUserWithActivities(int userId)
		{
			return ctx.Users.Include(user => user.Activities)
				.Where(user => user.UserId == userId).SingleOrDefault();
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
		public IEnumerable<Activity> ReadActivitiesForUser(int userId)
		{
			return ctx.Users.Include(user => user.Activities).
				Where(user => user.UserId == userId).SingleOrDefault().Activities.AsEnumerable();
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
		/// Normally we don't delete a user, but we keep the information
		/// </summary>
		public int DeleteUser(int userId)
		{
			User userToDelete = readUserWithActivities(userId);
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
		/// Normally we don't delete a users, but we keep the information
		/// </summary>
		public int DeleteUsers(IEnumerable<int> userIds)
		{
			foreach (int userid in userIds)
			{
				User userToDelete = readUserWithActivities(userid);
				ctx.Users.Remove(userToDelete);
			}
			return ctx.SaveChanges();
		}
	}
}
