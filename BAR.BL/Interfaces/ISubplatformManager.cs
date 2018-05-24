using BAR.BL.Domain.Core;
using BAR.BL.Domain.Users;
using System;
using System.Collections.Generic;
using System.Web;

namespace BAR.BL.Managers
{
	public interface ISubplatformManager
	{
		//Subplatsorms
		SubPlatform GetSubPlatform(int subplatformId);
		SubPlatform GetSubPlatform(string platformName);
		IEnumerable<SubPlatform> GetSubplatforms();

		SubPlatform AddSubplatform(string name);

		SubPlatform ChangeSubplatform(SubPlatform platform);

		void RemoveSubplatform(int platformId);

		//Customizations
		Customization GetCustomization(int subplatfromId);
		Customization ChangePageColors(int platformId, string primaryColor, string primaryDarkerColor, string primaryDarkestColor, string secondairyColor, string secondaryLighterColor, string secondaryDarkerColor, string secondaryDarkestColor, string tertiaryColor, string backgroundColor, string textColor);

		Customization ChangePageText(int platformId, string personAlias, string personsAlias, string organisationAlias, string organisationsAlias, string themeAlias, string themesAlias);
		Customization ChangePrivacyText(int platformId, string content, string title = "Privacy policy");
		Customization ChangeFAQTitle(int platformId, string title);
		Customization ChangeAddress(int platformId, string streetAndHousenumber, string zipcode, string city, string country, string email);
		Customization ChangeSubplatformHeaderImage(int platformId, HttpPostedFileBase headerImgFile);
		Customization ChangeSubplatformLogo(int platformId, HttpPostedFileBase logoImgFile);
		Customization ChangeSubplatformDarkLogo(int platformId, HttpPostedFileBase darkLogoImgFile);

		//Questions
		Question GetQuestion(int questionId);
		IEnumerable<Question> GetAllQuestions();
		IEnumerable<Question> GetQuestions(int subplatformId);
		IEnumerable<Question> GetQuestionsForType(QuestionType type);

		Question AddQuestion(int platformId, QuestionType type, string title, string anwser);

		Question ChangeQuestion(int questionId, QuestionType type, string title, string anwser);

		void RemoveQuestion(int questionId);

		bool Exists(int questionid);

		//Activities
		IEnumerable<UserActivity> GetUserActivities(ActivityType type, DateTime? timestamp = null);

		UserActivity AddUserActitity(ActivityType type, double numberOfUsers = 0.0);

		void LogActivity(ActivityType type);
	}
}
