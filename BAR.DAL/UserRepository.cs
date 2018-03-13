using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{ 
  public class UserRepository : IUserRepository
  {
    private List<User> users;

    public UserRepository()
    {
      users = new List<User>();
    }

    public User ReadUser(int userID)
    {
      User user = users.Where(u => u.UserId == userID).FirstOrDefault();

      return user;
    }
  }
} 
