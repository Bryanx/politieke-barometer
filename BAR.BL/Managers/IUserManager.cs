using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface IUserManager
	{
		User GetUser(string userId);
		IEnumerable<User> GetAllUsers();

		User ChangeUserBasicInfo(string userId, string firstname, string lastname, Gender gender, DateTime dateOfBrith, Area area);
		User ChangeUserAlerts(string userId, bool alertWebsite, bool alertMail, bool alertWeeklyReview);
		User ChangeUserAccount(string userId, bool active);
    IEnumerable<Area> GetAreas();
    Area GetArea(int areaId);
    IEnumerable<IdentityRole> GetAllRoles();
    IdentityRole GetRole(string userId);
  }
}
