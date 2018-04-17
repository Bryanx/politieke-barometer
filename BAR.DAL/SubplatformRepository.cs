﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq;
using BAR.BL.Domain;
using BAR.DAL.EF;
using BAR.BL.Domain.Core;

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
		/// Gives back a subplatform with all the questions and then customization.
		/// </summary>
		public SubPlatform GetSubplatformWithAllinfo(string subplatformName)
		{
			return ctx.SubPlatforms.Include(platform => platform.Questions)
								   .Include(platform => platform.Customization)
								   .Where(platform => platform.Name.ToLower().Equals(subplatformName.ToLower())).SingleOrDefault();
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
		public IEnumerable<Question> ReadQuestions(string subplatformName)
		{
			return ctx.Questions
				.Where(question => question.SubPlatform.Name.ToLower().Equals(subplatformName.ToLower())).AsEnumerable();
		}

		/// <summary>
		/// Reads all the questions form a specific type
		/// </summary>
		public IEnumerable<Question> ReadQuestionsForType(QuestionType type)
		{
			return ctx.Questions.Where(question => question.QuestionType == type).AsEnumerable();
		}

		/// <summary>
		/// Gives back the configuration of a specific subplatform
		/// </summary>
		public SubPlatform ReadSubplatformWithCustomization(string subplatformName)
		{
			return ctx.SubPlatforms.Include(platform => platform.Customization)
									.Where(platform => platform.Name.ToLower().Equals(subplatformName.ToLower())).SingleOrDefault();
		}

		/// <summary>;;
		/// Reads a subplatform based on name of the subplatform.
		/// </summary>
		public SubPlatform ReadSubPlatform(string subplatformName)
		{
			return ctx.SubPlatforms.Where(sp => sp.Name.Equals(subplatformName)).SingleOrDefault();
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
		/// Updates a given list of subplatforms
		/// </summary>
		public int UpdateSubplatforms(IEnumerable<SubPlatform> subPlatforms)
		{
			foreach (SubPlatform platform in subPlatforms) ctx.Entry(platform).State = EntityState.Modified;
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
		/// Deletes a subplatform.
		/// 
		/// NOTE
		/// When you delete a subplatform, all other information of
		/// that subplatform will also be deleted.
		/// </summary>
		public int DeleteSubplatform(SubPlatform subPlatform)
		{
			//*** WARNING ***//
			//This method does yet delete the whole platform.
			//It just deletes the instance of the subplatform
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
	}
}
