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
		public Customization ChangeContactProperties(string platformName, string streetAndHousenumber, string zipcode, string city, string country, string email)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformName);
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
		public Customization ChangeFAQTitle(string platformName, string title)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformName);
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
		public Customization ChangePageColors(string platformName, string primaryColor, string secondairyColor, string tertiaryColor, string backgroundColor, string textColor)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformName);
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
		public Customization ChangePageText(string platformName, string personAlias, string personsAlias, string organisationAlias, string organisationsAlias, string themeAlias, string themesAlias)
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformName);
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
		public Customization ChangePrivacyText(string platformName, string content, string title = "Privacy policy")
		{
			InitRepo();

			//Get Customization of platform
			SubPlatform platform = platformRepo.ReadSubplatformWithCustomization(platformName);
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
		public SubPlatform ChangePlatformName(string platformName, string name)
		{
			InitRepo();

			//Get platform
			SubPlatform platform = platformRepo.ReadSubPlatform(platformName);
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
		public SubPlatform CreateSubplatform(string name)
		{
			InitRepo();

			//Make subplatform
			SubPlatform platform = new SubPlatform()
			{
				Name = name,
				CreationDate = DateTime.Now,
				NumberOfUsers = 0,
				Questions = new List<Question>(),
				Customization = CreateDefaultCustomization()
			};

			//Create subplatform
			platformRepo.CreateSubplatform(platform);

			return platform;
		}

		private Customization CreateDefaultCustomization()
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
		/// Returns a speicif subplatform
		/// </summary>
		public SubPlatform GetSubPlatform(string subplatformName)
		{
			InitRepo();
			return platformRepo.ReadSubPlatform(subplatformName);
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
