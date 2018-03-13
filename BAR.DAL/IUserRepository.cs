using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
  public interface IUserRepository
  {
    User ReadUser(int userID);
  }
}
