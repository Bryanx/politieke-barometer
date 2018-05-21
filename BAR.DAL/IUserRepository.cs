using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BAR.DAL
{
	public interface IUserRepository
	{
		//Read
		User ReadUser(string userId);
		IEnumerable<User> ReadAllUsers();
		IEnumerable<User> ReadAllUsersWithAlerts();
		Area ReadArea(int areaId);
		IEnumerable<Area> ReadAllAreas();
		IdentityRole ReadRole(string userId);
		IEnumerable<IdentityRole> ReadAllRoles();

		//Create
		int CreateUser(User user);

		//update
		int UpdateUser(User user);
		int UpdateUsers(IEnumerable<User> users);

		//Delete
		//User acounts will be deactivated, but not deleted.
	}
}
