﻿using BAR.BL.Domain;
using BAR.BL.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	public interface ISubplatformManager
	{
		//Subplatsorms
		SubPlatform GetSubPlatform(string subplatformName);
		IEnumerable<SubPlatform> GetSubplatforms();

		SubPlatform CreateSubplatform(string name);

		SubPlatform ChangePlatformName(string platformName, string name);
		Customization ChangePageColors(string platformName, string primaryColor, string secondairyColor, string tertiaryColor, string backgroundColor, string textColor);
		Customization ChangePageText(string platformName, string personAlias, string personsAlias, string organisationAlias, string organisationsAlias, string themeAlias, string themesAlias);
		Customization ChangePrivacyText(string platformName, string content, string title = "Privacy policy");
		Customization ChangeFAQTitle(string platformName, string title);
		Customization ChangeContactProperties(string platformName, string streetAndHousenumber, string zipcode, string city, string country, string email);

		//Questions
	}
}
