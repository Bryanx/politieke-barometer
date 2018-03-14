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

		public UserManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		public User GetUser(int userId)
        {
            IUserRepository userRepo = new UserRepository();
            return userRepo.ReadUser(userId);
        }

		public void InitRepo()
		{
			if (uowManager == null) userRepo = new UserRepository();
			else userRepo = new UserRepository(uowManager.UnitOfWork);
		}
    }
}
