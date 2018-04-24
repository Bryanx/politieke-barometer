﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.BL.Domain;
using BAR.DAL;
using BAR.BL.Domain.Core;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsible for managing SubPlatform
	/// </summary>
	public class SubplatformManager : ISubplatformManager
	{
		private ISubplatformRepository platformRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public SubplatformManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Changes the contact properties of a specific platform
		/// Updates the changes in database
		/// </summary>
		public Customization ChangeAddress(int platformId, string streetAndHousenumber, string zipcode, string city, string country, string email)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformId);
			if (platform == null || platform.Customization == null) return null;

			//Change Customization
			Customization custom = platform.Customization;
			custom.StreetAndHousenumber = streetAndHousenumber;
			custom.Zipcode = zipcode;
			custom.City = city;
			custom.Country = country;
			custom.Email = email;

			//Update platform
			platformRepo.UpdateSubplatform(platform);

			return custom;
		}

		/// <summary>
		/// Changes the FAQ title of a specific platform
		/// Updates the changes in database
		/// </summary>
		public Customization ChangeFAQTitle(int platformId, string title)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformId);
			if (platform == null || platform.Customization == null) return null;

			//Change Customization
			Customization custom = platform.Customization;
			custom.FAQTitle = title;

			//Update platform
			platformRepo.UpdateSubplatform(platform);

			return custom;
		}

		/// <summary>
		/// Changes the page colors of a specific platform
		/// Updates the changes in database
		/// </summary>
		public Customization ChangePageColors(int platformId, string primaryColor, string secondairyColor, string tertiaryColor, string backgroundColor, string textColor)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformId);
			if (platform == null || platform.Customization == null) return null;

			//Change Customization
			Customization custom = platform.Customization;
			custom.PrimaryColor = primaryColor;
			custom.SecondairyColor = secondairyColor;
			custom.TertiaryColor = tertiaryColor;
			custom.BackgroundColor = backgroundColor;
			custom.TextColor = textColor;

			//Update platform
			platformRepo.UpdateSubplatform(platform);

			return custom;
		}

		/// <summary>
		/// Changes the page text of a specific platform
		/// Updates the changes in database
		/// </summary>
		public Customization ChangePageText(int platformId, string personAlias, string personsAlias, string organisationAlias, string organisationsAlias, string themeAlias, string themesAlias)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformId);
			if (platform == null || platform.Customization == null) return null;

			//Change Customization
			Customization custom = platform.Customization;
			custom.PersonAlias = personAlias;
			custom.PersonsAlias = personsAlias;
			custom.OrganisationAlias = organisationAlias;
			custom.OrganisationsAlias = organisationsAlias;
			custom.ThemeAlias = themeAlias;
			custom.ThemesAlias = themesAlias;

			//Update platform
			platformRepo.UpdateSubplatform(platform);

			return custom;
		}

		/// <summary>
		/// Changes the contact properties of a specific platform
		/// Updates the changes in database
		/// </summary>
		public Customization ChangePrivacyText(int platformId, string content, string title = "Privacy policy")
		{
			InitRepo();

			//Get Customization
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformId);
			if (platform == null || platform.Customization == null) return null;

			//Change Customization
			Customization custom = platform.Customization;
			custom.PrivacyText = content;
			custom.PrivacyTitle = title;

			//Update platform
			platformRepo.UpdateSubplatform(platform);

			return custom;
		}

		/// <summary>
		/// Changes the platformname of a specific platform
		/// Updates the changes in database
		/// 
		/// WARNING
		/// Method will not work because a manual change of the subdomain is required
		/// </summary>
		public SubPlatform ChangePlatformName(int platformId, string name)
		{
			InitRepo();

			//Get platform
			SubPlatform platform = GetSubPlatform(platformId);
			if (platform == null) return null;

			//Change platform
			platform.Name = name;

			//Update platform
			platformRepo.UpdateSubplatform(platform);

			return platform;
		}

		/// <summary>
		/// Makes an subplatform and persist that to the database
		/// </summary>
		public SubPlatform AddSubplatform(string name)
		{
			InitRepo();

			//Make subplatform
			SubPlatform platform = new SubPlatform()
			{
				Name = name,
				CreationDate = DateTime.Now,
				NumberOfUsers = 0,
				Questions = new List<Question>(),
				Customization = AddDefaultCustomization()
			};

			//Create subplatform
			platformRepo.CreateSubplatform(platform);

			return platform;
		}

		/// <summary>
		/// Gives back a default customization for a platform.
		/// </summary>
		private Customization AddDefaultCustomization()
		{
			Customization custom = new Customization()
			{
				//Colors
				PrimaryColor = "#0f8ec4",
				SecondairyColor = "#303E4D",
				TertiaryColor = "#278e87",
				BackgroundColor = "#f7f7f7",
				TextColor = "#73879C",

				//Navbar and title text
				PersonAlias = "Persoon",
				PersonsAlias = "Personen",
				OrganisationAlias = "Organisation",
				OrganisationsAlias = "Organisations",
				ThemeAlias = "Theme",
				ThemesAlias = "Themes",

				//Privacy
				PrivacyTitle = "Privacy policy",
				PrivacyText = "Copyright " + DateTime.Now.Year,

				//FAQ
				FAQTitle = "Frequently Asked Questions (FAQ)",

				//Contact properties
				StreetAndHousenumber = "Nationalestraat 24",
				Zipcode = "2060",
				City = "Antwerpen",
				Country = "België",
				Email = "contact@politiekebarometer.be"
			};

			return custom;
		}

		/// <summary>
		/// Returns a specific subplatform
		/// </summary>
		public SubPlatform GetSubPlatform(int platformId)
		{
			InitRepo();
			return platformRepo.ReadSubPlatform(platformId);
		}

		/// <summary>
		/// Returns a specific subplatform
		/// </summary>
		public SubPlatform GetSubPlatform(string platformName)
		{
			InitRepo();
			return platformRepo.ReadSubPlatform(platformName);
		}

		/// <summary>
		/// Returns all subplatforms
		/// </summary>
		public IEnumerable<SubPlatform> GetSubplatforms()
		{
			InitRepo();
			return platformRepo.ReadSubPlatforms().AsEnumerable();
		}

		/// <summary>
		/// Deletes a platform from the database
		/// </summary>
		public void RemoveSubplatform(int platformId)
		{
			InitRepo();

			//Get platform
			SubPlatform platform = GetSubPlatform(platformId);
			if (platform == null) return;

			//Delete platform
			platformRepo.DeleteSubplatform(platform);
		}

		/// <summary>
		/// Gives back a question based the questionId
		/// </summary>
		public Question GetQuestion(int questionId)
		{
			InitRepo();
			return platformRepo.ReadQuestion(questionId);
		}

		/// <summary>
		/// Gives back all the questions
		/// </summary>
		public IEnumerable<Question> GetAllQuestions()
		{
			InitRepo();
			return platformRepo.ReadAllQuestions().AsEnumerable();
		}

		/// <summary>
		/// Gives back all the questions for a specific subplatform
		/// </summary>
		public IEnumerable<Question> GetQuestions(int platformId)
		{
			InitRepo();
			return platformRepo.ReadQuestions(platformId).AsEnumerable();
		}

		/// <summary>
		/// Gives back all the questions for a specific questionstypeµ
		/// </summary>
		public IEnumerable<Question> GetQuestionsForType(QuestionType type)
		{
			InitRepo();
			return platformRepo.ReadQuestionsForType(type).AsEnumerable();
		}

		/// <summary>
		/// Creates a question in the database
		/// 
		/// WARNING
		/// THIS METHOD USES UNIT OF WORK
		/// 
		/// NOTE
		/// Unit of work is not realy needed because we work with one context
		/// Its added just for safety.
		/// </summary>
		public Question AddQuestion(int platformId, QuestionType type, string title, string anwser)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Get Subplatform
			SubPlatform platform = GetSubPlatform(platformId);
			if (platform == null) return null;

			//Create question
			Question question = new Question()
			{
				QuestionType = type,
				Title = title,
				Answer = anwser,
				SubPlatform = platform
			};

			//Create question in database
			platformRepo.CreateQuestion(question);
			uowManager.Save();
			uowManager = null;

			return question;
		}

		/// <summary>
		/// Changes a question and persist that question to the database
		/// </summary>
		public Question ChangeQuestion(int questionId, QuestionType type, string title, string anwser)
		{
			InitRepo();

			//Get question
			Question questionToUpdate = GetQuestion(questionId);
			if (questionToUpdate == null) return null;

			//Change question
			questionToUpdate.QuestionType = type;
			questionToUpdate.Title = title;
			questionToUpdate.Answer = anwser;

			//Update database
			platformRepo.UpdateQuestion(questionToUpdate);

			return questionToUpdate;
		}

		/// <summary>
		/// Removes a question from the database
		/// </summary>
		public void RemoveQuestion(int questionId)
		{
			InitRepo();

			//Get question
			Question question = GetQuestion(questionId);
			if (question == null) return;

			platformRepo.DeleteQuestion(question);
		}

		/// <summary>
		/// Gives back the customization for a specific subplatform.
		/// </summary>
		public Customization GetCustomization(int platformId)
		{
			InitRepo();
			return platformRepo.ReadSubplatformWithCustomization(platformId).Customization;
		}

		/// <summary>
		/// Returns true if the object exists in the database
		/// Returns fasle otherwise.
		/// </summary>
		public bool Exists(int questionid)
		{
			if (GetQuestion(questionid) != null) return true;
			else return false;
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) platformRepo = new SubplatformRepository();
			else platformRepo = new SubplatformRepository(uowManager.UnitOfWork);
		}
	}
}