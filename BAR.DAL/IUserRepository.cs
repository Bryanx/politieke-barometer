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
		int CreateUser(User user);

		//update
		int UpdateUser(User user);
		int UpdateUsers(IEnumerable<User> users);

		//Delete
		int DeleteUser(User user);
		int DeleteUsers(IEnumerable<User> users);
	}
}
