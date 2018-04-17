using BAR.BL.Domain;
using BAR.BL.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.DAL
{
	public interface ISubplatformRepository
	{
		//Read
		SubPlatform ReadSubPlatform(string subplatformName);
		SubPlatform ReadSubplatformWithCustomization(string subplatformName);
		IEnumerable<SubPlatform> ReadSubPlatform();

		Question ReadQuestion(int questionId);
		IEnumerable<Question> ReadAllQuestions();
		IEnumerable<Question> ReadQuestions(string subplatformName);
		IEnumerable<Question> ReadQuestionsForType(QuestionType type);
	
		//Create
		int CreateSubplatform(SubPlatform subPlatform);
		int CreateQuestion(Question question);

		//Update
		int UpdateSubplatform(SubPlatform subPlatform);
		int UpdateSubplatforms(IEnumerable<SubPlatform> subPlatforms);

		//Delete
		int DeleteSubplatform(SubPlatform subPlatform);
	}
}
