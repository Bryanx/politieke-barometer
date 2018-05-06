using System;
using System.Collections.Generic;
using BAR.BL.Domain.Users;
using BAR.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.IO;
using System.Linq;

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
	}
}