﻿using BAR.BL.Domain;
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
		SubPlatform ReadSubPlatform(int platformId);
		SubPlatform ReadSubPlatform(string platformId);
		SubPlatform GetSubplatformWithAllinfo(int platformId);
		SubPlatform ReadSubplatformWithCustomization(int platformId);		
		IEnumerable<SubPlatform> ReadSubPlatforms();
		Customization ReadCustomisation(int platformId);
		Question ReadQuestion(int questionId);
		IEnumerable<Question> ReadAllQuestions();
		IEnumerable<Question> ReadQuestions(int platformId);
		IEnumerable<Question> ReadQuestionsForType(QuestionType type);
        int ReadInterval(int platformId);
        String ReadStartTime(int platformId);
	
		//Create
		int CreateSubplatform(SubPlatform subPlatform);
		int CreateQuestion(Question question);

		//Update
		int UpdateSubplatform(SubPlatform subPlatform);
		int UpdateSubplatforms(IEnumerable<SubPlatform> subPlatforms);
		int UpdateQuestion(Question question);
		int UpdateQuestions(IEnumerable<Question> questions);
        int UpdateInterval(int platformId,int interval);
        int UpdateStartTime(int platformId,string setTime);
        
		//Delete
		int DeleteSubplatform(SubPlatform subPlatform);
		int DeleteQuestion(Question question);
		int DeleteQuestions(IEnumerable<Question> questions);
	}
}
