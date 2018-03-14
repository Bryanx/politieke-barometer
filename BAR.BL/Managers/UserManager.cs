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
        public User GetUser(int userId)
        {
            IUserRepository userRepo = new UserRepository();
            return userRepo.ReadUser(userId);
        }
    }
}
