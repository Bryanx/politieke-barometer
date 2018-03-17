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
		//Read
		User ReadUser(int userID);
		IEnumerable<User> ReadAllUsers();
		IEnumerable<User> ReadAllUsersForRole(int roleId);
		IEnumerable<User> ReadAllUsersForArea(int areaId);

		//Create
		//...

		//update
		//...

		//Delete
	}
}
