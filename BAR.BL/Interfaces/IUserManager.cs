using BAR.BL.Domain.Users;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web;

namespace BAR.BL.Managers
{
	public interface IUserManager
	{
		//Identity
		IdentityRole GetRole(string userId);
		IEnumerable<IdentityRole> GetAllRoles();

		//Users
		User GetUser(string userId);
		IEnumerable<User> GetAllUsers();

		User ChangeUserBasicInfo(string userId, string firstname, string lastname, Gender gender, DateTime dateOfBrith, Area area);
		User ChangeUserAlerts(string userId, bool alertWebsite, bool alertMail, bool alertWeeklyReview);
		User ChangeUserAccount(string userId);
		User ChangeProfilePicture(string userId, HttpPostedFileBase poImgFile);
		User ChangeBasicInfoAndroid(string userId, string firstname, string lastname, byte[] profilePicture = null);
		User ChangeDeviceToken(string userId, string deviceToken);
		bool GenerateAlertsForWeeklyReview(int platformId);

		//Areas
		Area GetArea(int areaId);
		IEnumerable<Area> GetAreas();
	}
}
