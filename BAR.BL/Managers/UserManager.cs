using System;
using System.Collections.Generic;
using BAR.BL.Domain.Users;
using BAR.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.IO;
using System.Linq;
using BAR.BL.Domain.Core;
using BAR.BL.Domain.Items;
using Microsoft.AspNet.Identity;

namespace BAR.BL.Managers
{
	/// <summary>
	/// This will handle all user related requests.
	/// </summary>
	public class UserManager : IUserManager
	{
		private IUserRepository userRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public UserManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) userRepo = new UserRepository();
			else userRepo = new UserRepository(uowManager.UnitOfWork);
		}

		/// <summary>
		/// Changes a user account to non-active or active.
		/// </summary>
		public User ChangeUserAccount(string userId)
		{
			InitRepo();

			//Get user
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change user (Toggle deleted state)
			userToUpdate.Deleted = !userToUpdate.Deleted;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}

		/// <summary>
		/// Changes the basic information of a specific user.
		/// 
		/// NOTE: This method changes the following things:
		/// - Receiving alerts via website
		/// - Receiving alerts via mail
		/// - Receiving weekly review via mail
		/// </summary>
		public User ChangeUserAlerts(string userId, bool alertWebsite, bool alertMail, bool alertWeeklyReview)
		{
			InitRepo();

			//Get user
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Update user
			userToUpdate.AlertsViaWebsite = alertWebsite;
			userToUpdate.AlertsViaEmail = alertMail;
			userToUpdate.WeeklyReviewViaEmail = alertWeeklyReview;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}

		/// <summary>
		/// Changes the basic information of a specific user
		/// 
		/// NOTE: This method changes the following things:
		/// - firstname
		/// - lastname
		/// - gender
		/// - date of birth
		/// - area
		/// </summary>
		public User ChangeUserBasicInfo(string userId, string firstname, string lastname, Gender gender, DateTime dateOfBirth, Area area)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Get User
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change user
			userToUpdate.FirstName = firstname;
			userToUpdate.LastName = lastname;
			userToUpdate.Gender = gender;
			userToUpdate.DateOfBirth = dateOfBirth;
			userToUpdate.Area = area;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			uowManager.Save();
			return userToUpdate;
		}

		/// <summary>
		/// Returns a list of all users.
		/// </summary>
		public IEnumerable<User> GetAllUsers()
		{
			InitRepo();
			return userRepo.ReadAllUsers().AsEnumerable();
		}

		/// <summary>
		/// Returns a user for a specific userId.
		/// </summary>
		public User GetUser(string userId)
		{
			InitRepo();
			return userRepo.ReadUser(userId);
		}

		/// <summary>
		/// Returns all areas.
		/// </summary>
		public IEnumerable<Area> GetAreas()
		{
			InitRepo();
			return userRepo.ReadAllAreas().AsEnumerable();
		}

		/// <summary>
		/// Returns selected area.
		/// </summary>
		public Area GetArea(int areaId)
		{
			InitRepo();
			return userRepo.ReadArea(areaId);
		}

		/// <summary>
		/// Returns a list of all roles.
		/// </summary>
		public IEnumerable<IdentityRole> GetAllRoles()
		{
			InitRepo();
			return userRepo.ReadAllRoles().AsEnumerable();
		}

		/// <summary>
		/// Returns role of a given user.
		/// </summary>
		public IdentityRole GetRole(string userId)
		{
			InitRepo();
			return userRepo.ReadRole(userId);
		}

		/// <summary>
		/// Changes profile picture of given user.
		/// </summary>
		public User ChangeProfilePicture(string userId, HttpPostedFileBase poImgFile)
		{
			InitRepo();

			//Get User
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change profile picture
			byte[] imageData = null;
			using (var binary = new BinaryReader(poImgFile.InputStream))
			{
				imageData = binary.ReadBytes(poImgFile.ContentLength);
			}
			userToUpdate.ProfilePicture = imageData;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}

		/// <summary>
		/// Changes the inforamtion of a user that has modified his
		/// name on a android device.
		/// </summary>
		public User ChangeBasicInfoAndroid(string userId, string firstname, string lastname, byte[] profilePicture = null)
		{
			InitRepo();

			//Get User
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change user
			userToUpdate.FirstName = firstname;
			userToUpdate.LastName = lastname;
			userToUpdate.ProfilePicture = profilePicture;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}

		/// <summary>
		/// Generate alerts for the weekly review
		/// </summary>
		public bool GenerateAlertsForWeeklyReview(int platformId)
		{
			InitRepo();

			//Get timepstamp for weekly review
			SubplatformManager platformManager = new SubplatformManager();
			SubPlatform platform = platformManager.GetSubPlatform(platformId);
			if (platform.LastUpdatedWeeklyReview != null && platform.LastUpdatedWeeklyReview > DateTime.Now.AddDays(-7)) return false;

			platform.LastUpdatedWeeklyReview = DateTime.Now;
			platformManager.ChangeSubplatform(platform);

			//Get all users
			IEnumerable<User> users = userRepo.ReadAllUsersWithAlerts();
			if (users == null || users.Count() == 0) return false;

			//Generate weekly review alerts
			foreach (User user in users)
			{
				UserAlert alert = new UserAlert()
				{
					User = user,
					Subject = "Nieuwe Weekly Review",
					IsRead = false,
					TimeStamp = DateTime.Now,
					AlertType = AlertType.Weekly_Review
				};
				user.Alerts.Add(alert);
			}

			//Update database & send emails
			SendWeeklyReviewEmails(platformId, users.Where(user => user.AlertsViaEmail));
			userRepo.UpdateUsers(users);
			return true;
		}

		/// <summary>
		/// Sent emails to the people who want to receive an email
		/// </summary>
		private void SendWeeklyReviewEmails(int subplatformId, IEnumerable<User> users)
		{
			ItemManager itemManager = new ItemManager();
			IEnumerable<Item> items;

			foreach (User user in users)
			{
				//Get 5 most trending items of
				items = itemManager.GetMostTrendingItemsForUser(subplatformId, user.Id, useWithOldData: true);

				string content = "";
				foreach (Item item in items.OrderBy(item => item.TrendingPercentage)) content += "- " + item.Name + " (" + item.TrendingPercentage + "% trending)</br>";

				//Send email
				IdentityMessage message = new IdentityMessage()
				{
					Destination = user.Email,
					Subject = "Nieuwe Weekly Review is nu beschikbaar!",
					Body = "Beste " + user.FirstName + "</br></br>" +
						"Een nieuwe weekly review is nu beschikbaar!</br></br>" +
						"De <strong>meest trending items</strong> van deze week zijn:</br>" +
						content +
						"</br>Ga nu naar onze website om uw nieuwe Weekly Review te bekijken!"
				};
				new EmailService().Send(message);
			}
		}

		/// <summary>
		/// Changes the device token to the device where the user logs in.
		/// </summary>
		public User ChangeDeviceToken(string userId, string deviceToken)
		{
			InitRepo();

			//Get User
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change user device token
			userToUpdate.DeviceToken = deviceToken;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}
	}
}