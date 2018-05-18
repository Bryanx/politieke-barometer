using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAR.BL.Managers
{
	public interface IUserManager
	{
		User GetUser(string userId);
		IEnumerable<User> GetAllUsers();
		IEnumerable<Area> GetAreas();
		Area GetArea(int areaId);
		IEnumerable<IdentityRole> GetAllRoles();
		IdentityRole GetRole(string userId);

		User ChangeUser(User user);
		User ChangeUserBasicInfo(string userId, string firstname, string lastname, Gender gender, DateTime dateOfBrith, Area area);
		User ChangeUserAlerts(string userId, bool alertWebsite, bool alertMail, bool alertWeeklyReview);
		User ChangeUserAccount(string userId);
		User ChangeProfilePicture(string userId, HttpPostedFileBase poImgFile);
		User ChangeBasicInfoAndroid(string userId, string firstname, string lastname, byte[] profilePicture = null);
    User ChangeDeviceToken(string userId, string deviceToken);

		bool GenerateAlertsForWeeklyReview(int platformId);
	}
}
