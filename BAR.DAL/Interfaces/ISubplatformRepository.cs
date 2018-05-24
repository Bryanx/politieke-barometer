using BAR.BL.Domain.Core;
using BAR.BL.Domain.Users;
using System.Collections.Generic;

namespace BAR.DAL
{
	public interface ISubplatformRepository
	{
		//Read
		SubPlatform ReadSubPlatform(int platformId);
		SubPlatform ReadSubPlatform(string platformId);
		SubPlatform ReadSubplatformWithCustomization(int platformId);
		IEnumerable<SubPlatform> ReadSubPlatforms();
		Customization ReadCustomisation(int platformId);
		Question ReadQuestion(int questionId);
		IEnumerable<Question> ReadAllQuestions();
		IEnumerable<Question> ReadQuestions(int platformId);
		IEnumerable<Question> ReadQuestionsForType(QuestionType type);
		IEnumerable<UserActivity> ReadAllActvities();
		IEnumerable<UserActivity> ReadActivitiesForType(ActivityType type);

		//Create
		int CreateSubplatform(SubPlatform subPlatform);
		int CreateQuestion(Question question);
		int CreateUserActitivy(UserActivity actitivy);

		//Update
		int UpdateSubplatform(SubPlatform subPlatform);
		int UpdateQuestion(Question question);
		int UpdateQuestions(IEnumerable<Question> questions);
		int UpdateUserActivity(UserActivity activity);
		int UpdateCustomization(Customization customization);

		//Delete
		int DeleteSubplatform(SubPlatform subPlatform);
		int DeleteQuestion(Question question);
		int DeleteQuestions(IEnumerable<Question> questions);
	}
}
