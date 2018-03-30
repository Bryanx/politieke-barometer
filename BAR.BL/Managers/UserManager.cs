﻿using BAR.BL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain.Users;
using BAR.DAL;

namespace BAR.BL
{
	/// <summary>
	/// This class is mainly for workig with a manager
	/// that is not related to the identity framework.
	/// </summary>
	public class UserManager : IUserManager
	{
		private UserRepository userRepo;
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
		/// Changes a user account to non-active or active
		/// </summary>
		public User ChangeUserAccount(string userId, bool active)
		{
			InitRepo();

			//Get user
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change user
			userToUpdate.IsActive = active;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}

		/// <summary>
		/// Changes the basic information of a specific user
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
		/// </summary>
		public User ChangeUserBasicInfo(string userId, string firstname, string lastname, Gender gender, DateTime dateOfBrith)
		{
			InitRepo();

			//Get User
			User userToUpdate = userRepo.ReadUser(userId);
			if (userToUpdate == null) return null;

			//Change user
			userToUpdate.FirstName = firstname;
			userToUpdate.LastName = lastname;
			userToUpdate.Gender = gender;
			userToUpdate.DateOfBirth = dateOfBrith;

			//Update database
			userRepo.UpdateUser(userToUpdate);
			return userToUpdate;
		}

		/// <summary>
		/// Returns a list of all users.
		/// </summary>
		public IEnumerable<User> GetAllUsers()
		{
			InitRepo();
			return userRepo.ReadAllUsers();
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
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) userRepo = new UserRepository();
			else userRepo = new UserRepository(null, uowManager.UnitOfWork);
		}
	}
}
