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
		User ReadUser(int userId);
		User readUserWithActivities(int userId);
		IEnumerable<User> ReadAllUsers();
		IEnumerable<User> ReadAllUsersForRole(int roleId);
		IEnumerable<User> ReadAllUsersForArea(int areaId);
		IEnumerable<Activity> ReadAllActivities();
		IEnumerable<Activity> ReadActivitiesForUser(int userId);

		//Create
		int CreateUser(User user);

		//update
		int UpdateUser(User user);
		int UpdateUsers(IEnumerable<User> users);

		//Delete
		int DeleteUser(int userId);
		int DeleteUsers(IEnumerable<int> userIds);
	}
}
