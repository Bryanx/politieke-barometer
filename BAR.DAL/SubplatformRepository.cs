using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BAR.DAL.EF;
using BAR.BL.Domain.Core;
using BAR.BL.Domain.Users;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of
	/// subplatforms.
	/// </summary>
	public class SubplatformRepository : ISubplatformRepository
	{
		private readonly BarometerDbContext ctx;

		/// <summary>
		/// If uow is present then the constructor
		/// will get the context from uow.
		/// </summary>
		public SubplatformRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}

		/// <summary>
		/// Creates a new instance of a given subplatform in the database
		/// </summary>
		public int CreateSubplatform(SubPlatform subPlatform)
		{
			ctx.SubPlatforms.Add(subPlatform);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Creates an instance of a question
		/// </summary>
		public int CreateQuestion(Question question)
		{
			ctx.Questions.Add(question);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Reads a specific question.
		/// </summary>
		public Question ReadQuestion(int questionId)
		{
			return ctx.Questions.Find(questionId);
		}

		/// <summary>
		/// Reads all the questions
		/// </summary>
		public IEnumerable<Question> ReadAllQuestions()
		{
			return ctx.Questions.AsEnumerable();
		}

		/// <summary>
		/// Reads all the questions from a specific subplatform.
		/// </summary>
		public IEnumerable<Question> ReadQuestions(int platformId)
		{
			return ctx.Questions.Where(question => question.SubPlatform.SubPlatformId == platformId)
								.AsEnumerable();
		}

		/// <summary>
		/// Reads all the questions form a specific type
		/// </summary>
		public IEnumerable<Question> ReadQuestionsForType(QuestionType type)
		{
			return ctx.Questions.Where(question => question.QuestionType == type)
								.AsEnumerable();
		}

		/// <summary>
		/// Gives back the configuration of a specific subplatform
		/// </summary>
		public SubPlatform ReadSubplatformWithCustomization(int platformId)
		{
			return ctx.SubPlatforms.Include(platform => platform.Customization)
								   .Where(platform => platform.SubPlatformId == platformId)
								   .SingleOrDefault();
		}

		/// <summary>;;
		/// Reads a subplatform based on id of the subplatform.
		/// </summary>
		public SubPlatform ReadSubPlatform(int platformId)
		{
			return ctx.SubPlatforms.Find(platformId);
		}

		/// <summary>
		/// Reads a subplatform based on name of the subplatform.
		/// </summary>
		public SubPlatform ReadSubPlatform(string platformName)
		{
			return ctx.SubPlatforms.Where(platform => platform.Name.ToLower().Equals(platformName.ToLower()))
								   .SingleOrDefault();
		}

		/// <summary>
		/// Gives back all the subplatforms that are on the system
		/// </summary>
		public IEnumerable<SubPlatform> ReadSubPlatforms()
		{
			return ctx.SubPlatforms.AsEnumerable();
		}

		/// <summary>
		/// Updates a single subplatform.
		/// </summary>
		public int UpdateSubplatform(SubPlatform subPlatform)
		{
			ctx.Entry(subPlatform).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a specific question in the database
		/// </summary>
		public int UpdateQuestion(Question question)
		{
			ctx.Entry(question).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a given list of questions in the database
		/// </summary>
		public int UpdateQuestions(IEnumerable<Question> questions)
		{
			foreach (Question question in questions) ctx.Entry(question).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a customization object in the database
		/// </summary>
		public int UpdateCustomization(Customization customization)
		{
			ctx.Entry(customization).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a subplatform.
		/// 
		/// NOTE
		/// When you delete a subplatform, all other information of
		/// that subplatform will also be deleted.
		/// </summary>
		public int DeleteSubplatform(SubPlatform subPlatform)
		{
			ctx.SubPlatforms.Remove(subPlatform);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a specific question in the database
		/// </summary>
		public int DeleteQuestion(Question question)
		{
			ctx.Questions.Remove(question);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a given list of questions
		/// </summary>
		public int DeleteQuestions(IEnumerable<Question> questions)
		{
			foreach (Question question in questions) ctx.Questions.Remove(question);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back the customisation for a specific subplatform
		/// </summary>
		public Customization ReadCustomisation(int platformId)
		{
			SubPlatform subplatFrom = ctx.SubPlatforms.Include(platform => platform.Customization)
													  .Where(platform => platform.SubPlatformId == platformId)
													  .SingleOrDefault();
			return subplatFrom.Customization;
		}

		/// <summary>
		/// Returnes all the activities
		/// </summary>
		public IEnumerable<UserActivity> ReadAllActvities()
		{
			return ctx.UserActivities.AsEnumerable();
		}

		/// <summary>
		/// Creates a new useractivity in the database
		/// </summary>
		public int CreateUserActitivy(UserActivity actitivy)
		{
			ctx.UserActivities.Add(actitivy);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates an actitiy in the database
		/// </summary>
		public int UpdateUserActivity(UserActivity activity)
		{
			ctx.Entry(activity).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back all activities for a specific type
		/// </summary>
		public IEnumerable<UserActivity> ReadActivitiesForType(ActivityType type)
		{
			return ctx.UserActivities.Where(act => act.ActivityType == type).AsEnumerable();
		}
	}
}
