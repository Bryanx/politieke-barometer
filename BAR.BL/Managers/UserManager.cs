using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Users;
using BAR.DAL;

namespace BAR.BL.Managers
{
	public class UserManager : IUserManager
	{
		private UnitOfWorkManager uowManager;
		private UserRepository userRepo;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public UserManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Returns a list of all users.
		/// </summary>
		public IEnumerable<User> GetAllUsers()
		{
			InitRepo();
			return userRepo.ReadAllUsers();
		}

		/// <summary>
		/// Creates a user
		/// </summary>
		public int CreateUser(User user)
		{
			InitRepo();
			return userRepo.CreateUser(user);
		}
		
		/// <summary>
		/// Returns a user for a specific userId.
		/// </summary>
		public User GetUser(int userId)
		{
			InitRepo();
			return userRepo.ReadUser(userId);
		}
		
		/// <summary>
		/// Returns a user for a specific userId.
		/// </summary>
		public void ChangeUser(User user)
		{
			InitRepo();
			userRepo.UpdateUser(user);
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) userRepo = new UserRepository();
			else userRepo = new UserRepository(uowManager.UnitOfWork);
		}
	}
}
