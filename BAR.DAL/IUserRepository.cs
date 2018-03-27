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
		User ReadUser(string userId);
		User ReadUserWithActivities(string userId);
		IEnumerable<User> ReadAllUsers();
		IEnumerable<User> ReadAllUsersForRole(string roleId);
		IEnumerable<User> ReadAllUsersForArea(int areaId);
		IEnumerable<Activity> ReadAllActivities();
		IEnumerable<Activity> ReadActivitiesForUser(string userId);

		//Create
		int CreateUser(User user);

		//update
		int UpdateUser(User user);
		int UpdateUsers(IEnumerable<User> users);

		//Delete
		int DeleteUser(string userId);
		int DeleteUsers(IEnumerable<string> userIds);
	}
}
