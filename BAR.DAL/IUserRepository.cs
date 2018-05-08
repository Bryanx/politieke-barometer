using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
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
		IEnumerable<User> ReadAllUsers();
		IEnumerable<User> ReadAllUsersForRole(string roleId);
		IEnumerable<User> ReadAllUsersForArea(int areaId);
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
		int DeleteUser(string userId);
		int DeleteUsers(IEnumerable<string> userIds);
	}
}
